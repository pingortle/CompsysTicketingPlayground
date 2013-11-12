using AdmitOne.Domain;
using AdmitOne.Domain.Entities;
using AdmitOne.Persistence;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace AdmitOne.ViewModel
{
    public sealed class MyTicketsViewModel : ReactiveObject, IRoutableViewModel
    {
        public MyTicketsViewModel(IScreen screen, ISession session)
        {
            HostScreen = screen;
            GoBack = HostScreen.Router.NavigateBack;

            Tickets = new ReactiveList<Ticket>();
            Employees = new ReactiveList<Employee>();

            _isFetchingTickets = session.IsWorking
                .ToProperty(this, x => x.IsFetchingTickets);

            this.WhenAnyValue(x => x.SelectedEmployee)
                .Where(x => x != null)
                .Subscribe(x =>
                    {
                        Tickets.Clear();
                        session.FetchResults(
                        new Query<Ticket, TicketWithLatestEvent>(q =>
                            (from t in q
                             join e in q.SelectMany(y =>
                                 y.TicketEvents
                                    .OrderByDescending(ev => ev.Time)
                                    .Take(1))
                             on t.Id equals e.TicketId
                             where e.EmployeeId == x.Id || x.Id == int.MinValue
                             select new TicketWithLatestEvent
                             {
                                 Description = t.Description,
                                 CustomerId = t.CustomerId,
                                 EmployeeId = e.EmployeeId,
                                 TicketId = t.Id,
                                 TicketStatus = e.TicketStatus,
                                 Time = e.Time
                             })))
                             .ObserveOn(RxApp.MainThreadScheduler)
                             .Select(y => new Ticket { Description = y.Description })
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

        public IReactiveCollection<Ticket> Tickets { get; set; }
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
