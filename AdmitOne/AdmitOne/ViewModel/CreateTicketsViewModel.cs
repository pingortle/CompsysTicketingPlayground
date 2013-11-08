using AdmitOne.Domain;
using AdmitOne.Persistence;
using ReactiveUI;
using System;
using System.Collections;
using System.Linq;
using System.Reactive.Linq;

namespace AdmitOne.ViewModel
{
    public class CreateTicketsViewModel : ReactiveObject, IRoutableViewModel
    {
        public CreateTicketsViewModel(ISession session, IScreen screen = null)
        {
            #region Initialization
            HostScreen = screen ?? new DefaultScreen(RxApp.DependencyResolver);

            GoBack = HostScreen.Router.NavigateBack;
            
            CurrentBatch = new ReactiveList<TicketItemViewModel>();
            Customers = new ReactiveList<Customer>();
            #endregion

            #region Populate Customer List
            session.FetchResults<Customer>()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(c => Customers.Add(c));
            #endregion

            #region Wire Up Commands
            AddTicket = new ReactiveCommand(
                this.WhenAny(
                x => x.CurrentBatch,
                y => y.TicketText,
                (x, y) => x.Value.Count() <= 100 && !string.IsNullOrWhiteSpace(y.Value)));
            AddTicket.Subscribe(_ =>
                {
                    CurrentBatch.Add(new TicketItemViewModel(TicketText, CurrentBatch.Remove));
                    TicketText = default(string);
                });

            var anyInList = CurrentBatch.Changed.Select(_ => CurrentBatch.Any());
            var isCustomerSelected = this.WhenAnyValue(x => x.SelectedCustomer).Select(x => x != null);
            var shouldSaveChanges = Observable.CombineLatest(anyInList, isCustomerSelected, (x, y) => x && y);

            SaveChanges = new ReactiveCommand(shouldSaveChanges.StartWith(false));

            _isExecuting = session.IsWorking.ToProperty(this, x => x.IsExecuting, false);

            SaveChanges.Select(x => CurrentBatch.ToList())
                .Subscribe(x =>
                    {
                        var tickets = session.GetStoreOf<Ticket>();
                        using (INotifyWhenComplete token = session.ScopedChanges())
                        {
                            token.Completion
                                .ObserveOn(RxApp.MainThreadScheduler)
                                .Subscribe(b => { if (b) (CurrentBatch as IList).Clear(); });

                            foreach (var item in x)
                            {
                                tickets.Add(new Ticket
                                {
                                    Description = item.Text,
                                    CustomerId = SelectedCustomer.Id
                                });
                            }
                        }
                    });
            #endregion
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get
            {
                return _selectedCustomer;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedCustomer, value);
            }
        }

        public IReactiveList<Customer> Customers { get; private set; }

        private ObservableAsPropertyHelper<bool> _isExecuting;
        public bool IsExecuting { get { return _isExecuting.Value; } }

        public IReactiveCommand AddTicket { get; private set; }

        public IReactiveCommand SaveChanges { get; private set; }

        private string _ticketText;
        public string TicketText
        {
            get { return _ticketText; }
            set { this.RaiseAndSetIfChanged(ref _ticketText, value); }
        }

        public IReactiveList<TicketItemViewModel> CurrentBatch { get; private set; }

        public IReactiveCommand GoBack { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "create_tickets"; }
        }
    }
}
