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
                        (x.Id == int.MinValue ? session.FetchResults<Ticket>() :
                        session.FetchResults(
                        new Query<Ticket>(q =>
                            (from t in q
                             where t.TicketEvents
                                .OrderByDescending(e => e.Time)
                                .FirstOrDefault().Employee.Id == x.Id
                             select t))))
                             .ObserveOn(RxApp.MainThreadScheduler)
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
