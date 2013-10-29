using AdmitOne.Data.Domain;
using AdmitOne.Data.Protozoa;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;

namespace AdmitOne.ViewModel
{
    public class CreateTicketsViewModel : ReactiveObject, IRoutableViewModel
    {
        public CreateTicketsViewModel(IScreen screen, IStore<Ticket> ticketBox, IStore<Customer> customerBox)
        {
            HostScreen = screen ?? new DefaultScreen(RxApp.DependencyResolver);

            GoBack = HostScreen.Router.NavigateBack;

            CurrentBatch = new ReactiveList<TicketItemViewModel>();

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

            SaveChanges = new ReactiveCommand(anyInList.StartWith(false));

            _isExecuting = SaveChanges.IsExecuting.ToProperty(this, x => x.IsExecuting, false);

            SaveChanges
                .Select(x => CurrentBatch.ToList())
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Select(x =>
                    {
                        try
                        {
                            foreach (var t in x)
                            {
                                var customer = customerBox.SingleOrDefault(m => m.Id == 1);
                                var ticket = new Ticket { Description = t.Text };
                                customer.Tickets.Add(ticket);
                                customerBox.Update(customer);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            return false;
                        }

                        return true;
                    })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                    {
                        if (x)
                            (CurrentBatch as IList).Clear();
                    });
        }

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
