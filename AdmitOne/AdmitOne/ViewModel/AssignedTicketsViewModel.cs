using AdmitOne.Domain;
using AdmitOne.Domain.Entities;
using AdmitOne.Persistence;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace AdmitOne.ViewModel
{
    public sealed class AssignedTicketsViewModel : ReactiveObject, IRoutableViewModel
    {
        public AssignedTicketsViewModel(IScreen screen, ISession session)
        {
            HostScreen = screen;
            GoBack = HostScreen.Router.NavigateBack;

            Tickets = new ReactiveList<TicketItemViewModel>();
            Employees = new ReactiveList<Employee>();

            _isFetchingTickets = session.IsWorking
                .ToProperty(this, x => x.IsFetchingTickets);

            this.WhenAnyValue(x => x.SelectedEmployee)
                .Where(x => x != null)
                .Subscribe(x =>
                    {
                        Tickets.Clear();

                        session.FetchMergedResults<Ticket, TicketEvent, TicketWithLatestEvent>((tickets, events) =>
                            from t in tickets
                            join e in events
                                .GroupBy(y => y.TicketId)
                                .SelectMany(g =>
                                    g.OrderByDescending(y => y.Time).Take(1))
                            on t.Id equals e.TicketId
                            orderby e.Time descending
                            select new TicketWithLatestEvent
                            {
                                Description = t.Description,
                                CustomerId = t.CustomerId,
                                EmployeeId = e.EmployeeId,
                                TicketId = t.Id,
                                TicketStatus = e.TicketStatus,
                                Time = e.Time
                            })
                             .ObserveOn(RxApp.MainThreadScheduler)
                             .Select(y => new TicketItemViewModel(y.Description, y.TicketStatus ?? TicketStatus.Open))
                             .Subscribe(y => Tickets.Add(y));
                    });

            SelectedEmployee = new Employee { Name = "All", TicketEvents = null, Id = int.MinValue };
            Employees.Add(SelectedEmployee);

            session.FetchResults<Employee>()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => Employees.Add(x));

            session.ThrownExceptions.Subscribe(x =>
                {
                    Console.WriteLine(x.Message);
                });
        }

        private ObservableAsPropertyHelper<bool> _isFetchingTickets;
        public bool IsFetchingTickets { get { return _isFetchingTickets.Value; } }

        public IReactiveCollection<TicketItemViewModel> Tickets { get; set; }
        public IReactiveCollection<Employee> Employees { get; set; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { this.RaiseAndSetIfChanged(ref _selectedEmployee, value); }
        }

        public IReactiveCommand GoBack { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment { get { return "myTickets"; } }
    }
}
