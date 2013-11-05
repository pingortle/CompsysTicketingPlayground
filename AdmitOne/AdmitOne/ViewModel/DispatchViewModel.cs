using AdmitOne.Domain;
using AdmitOne.Persistence;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace AdmitOne.ViewModel
{
    public sealed class DispatchViewModel : ReactiveObject, IRoutableViewModel
    {
        public DispatchViewModel(IScreen screen, ISession session)
        {
            HostScreen = screen;
            GoBack = HostScreen.Router.NavigateBack;

            Techs = new ReactiveList<string>();
            Tickets = new ReactiveList<string>();

            var getFreshTechs = new ReactiveCommand();
            var gotFreshTechs = getFreshTechs.RegisterAsyncFunction(_ => session.GetStoreOf<Employee>().Select(x => x.Name).ToList());
            gotFreshTechs.Subscribe(x => x.ForEach(t => Techs.Add(t)));

            var getFreshTickets = new ReactiveCommand();
            var gotFreshTickets = getFreshTickets.RegisterAsyncFunction(_ => session.GetStoreOf<Ticket>().Select(x => x.Description).ToList());
            gotFreshTickets.Subscribe(x => x.ForEach(t => Tickets.Add(t)));

            IEnumerable<IReactiveCommand> dataAccessCommands = new[] { getFreshTechs, getFreshTickets };
            var masterCommand = JoinMutuallyExclusiveAsyncCommands(
                dataAccessCommands,
                new[] { gotFreshTechs, gotFreshTickets });

            Refresh = masterCommand;
            Refresh.Subscribe(_ => { Techs.Clear(); Tickets.Clear(); });

            _error = Observable.Merge(dataAccessCommands.Select(x => x.ThrownExceptions))
                .Select(x => x.Message)
                .ToProperty(this, x => x.Error);

            masterCommand.Execute(default(object));
        }

        public IReactiveCommand JoinMutuallyExclusiveAsyncCommands(IEnumerable<IReactiveCommand> commands, IEnumerable<IObservable<object>> asyncResults)
        {
            if (!commands.Any() || !asyncResults.Any()) throw new ArgumentException("Enumerables must have elements.");
            if (commands.Count() != asyncResults.Count()) throw new ArgumentException("Enumerables must have equal numbers of elements.");

            var masterCommand = new ReactiveCommand(Observable.Merge(commands.Select(x => x.CanExecuteObservable)));

            masterCommand.RegisterAsyncAction(x =>
            {
                commands.Zip(asyncResults, (n, m) => new { Command = n, ObservableResult = m }).Aggregate((a, b) =>
                    {
                        b.ObservableResult.Take(1).Subscribe(_ => a.Command.Execute(x));
                        return b;
                    })
                    .Command.Execute(x);
            });

            return masterCommand;
        }

        public IReactiveCollection<string> Techs { get; private set; }
        public IReactiveCollection<string> Tickets { get; private set; }

        public IReactiveCommand GoBack { get; private set; }
        public IReactiveCommand Refresh { get; private set; }

        private ObservableAsPropertyHelper<string> _error;
        public string Error { get { return _error.Value; } }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment { get { return "dispatch"; } }
    }
}
