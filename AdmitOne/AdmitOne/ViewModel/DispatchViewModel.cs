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

            Techs = new ReactiveList<Employee>();
            Tickets = new ReactiveList<Ticket>();

            var getFreshTechs = new ReactiveCommand();
            getFreshTechs.ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => Techs.Clear());

            var gotFreshTechs = getFreshTechs.RegisterAsyncFunction(_ => session.GetStoreOf<Employee>().ToList());
            gotFreshTechs.Subscribe(x => x.ForEach(t => Techs.Add(t)));

            var getFreshTickets = new ReactiveCommand();
            getFreshTickets.ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => Tickets.Clear());

            var gotFreshTickets = getFreshTickets.RegisterAsyncFunction(_ => session.GetStoreOf<Ticket>().ToList());
            gotFreshTickets.Subscribe(x => x.ForEach(t => Tickets.Add(t)));

            IEnumerable<IReactiveCommand> dataAccessCommands = new[] { getFreshTechs, getFreshTickets };
            var masterCommand = dataAccessCommands.JoinMutuallyExclusiveAsyncCommands(new IObservable<object>[]
            {
                gotFreshTechs,
                gotFreshTickets
            });

            Refresh = masterCommand;

            // This illustrates the need for a mechanism to lock down the commands which reference a context.
            // Maybe it should be a view layer ReactiveCommand implementation?
            Assign = new ReactiveCommand(Observable.CombineLatest(
                this.WhenAny(
                    x => x.SelectedEmployee,
                    y => y.SelectedTicket,
                    (x, y) => x.Value != null && y.Value != null),
                masterCommand.CanExecuteObservable,
                (x, y) => x && y));
            Assign.RegisterAsyncAction(_ =>
            {
                var events = session.GetStoreOf<TicketEvent>();
                using (session.ScopedChanges())
                {
                    events.Add(new TicketEvent { Employee = SelectedEmployee, Ticket = SelectedTicket, TicketStatus = TicketStatus.Assigned, Time = DateTime.Now });
                }
            });

            _error = Observable.Merge(dataAccessCommands.Select(x => x.ThrownExceptions))
                .Select(x => x.Message)
                .ToProperty(this, x => x.Error);

            masterCommand.Execute(default(object));
        }

        public IReactiveCollection<Employee> Techs { get; private set; }
        public IReactiveCollection<Ticket> Tickets { get; private set; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { this.RaiseAndSetIfChanged(ref _selectedEmployee, value); }
        }

        private Ticket _selectedTicket;
        public Ticket SelectedTicket
        {
            get { return _selectedTicket; }
            set { this.RaiseAndSetIfChanged(ref _selectedTicket, value); }
        }

        public IReactiveCommand GoBack { get; private set; }
        public IReactiveCommand Refresh { get; private set; }
        public IReactiveCommand Assign { get; private set; }

        private ObservableAsPropertyHelper<string> _error;
        public string Error { get { return _error.Value; } }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment { get { return "dispatch"; } }
    }
}
