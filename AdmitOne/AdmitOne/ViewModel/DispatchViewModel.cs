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
            getFreshTechs.ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => Techs.Clear());

            var gotFreshTechs = getFreshTechs.RegisterAsyncFunction(_ => session.GetStoreOf<Employee>().Select(x => x.Name).ToList());
            gotFreshTechs.Subscribe(x => x.ForEach(t => Techs.Add(t)));

            var getFreshTickets = new ReactiveCommand();
            getFreshTickets.ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => Tickets.Clear());

            var gotFreshTickets = getFreshTickets.RegisterAsyncFunction(_ => session.GetStoreOf<Ticket>().Select(x => x.Description).ToList());
            gotFreshTickets.Subscribe(x => x.ForEach(t => Tickets.Add(t)));

            IEnumerable<IReactiveCommand> dataAccessCommands = new[] { getFreshTechs, getFreshTickets };
            var masterCommand = dataAccessCommands.JoinMutuallyExclusiveAsyncCommands(new[]
            {
                gotFreshTechs,
                gotFreshTickets
            });

            Refresh = masterCommand;

            _error = Observable.Merge(dataAccessCommands.Select(x => x.ThrownExceptions))
                .Select(x => x.Message)
                .ToProperty(this, x => x.Error);

            masterCommand.Execute(default(object));
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
