using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace AdmitOne.ViewModel
{
    public class CreateTicketsViewModel : ReactiveObject, IRoutableViewModel
    {
        public CreateTicketsViewModel(IScreen screen = null)
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
        }

        public IReactiveCommand AddTicket { get; private set; }

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
