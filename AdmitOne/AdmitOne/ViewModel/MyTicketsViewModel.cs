using AdmitOne.Data.Domain;
using AdmitOne.Data.Protozoa;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.ViewModel
{
    public sealed class MyTicketsViewModel : ReactiveObject, IRoutableViewModel
    {
        public MyTicketsViewModel(IScreen screen, ISee<Ticket> ticketViewer)
        {
            HostScreen = screen;
            GoBack = HostScreen.Router.NavigateBack;

            //var repo = new TicketRepository2();
            Tickets = new ReactiveList<TicketItemViewModel>();
            
            var refreshTickets = new ReactiveCommand();
            refreshTickets
                .RegisterAsyncFunction(_ =>
                    ticketViewer.AsEnumerable()
                    .Select(x => new TicketItemViewModel(x.Description)))
                .Subscribe(x =>
                    {
                        ReactiveList<TicketItemViewModel> tickets = (ReactiveList<TicketItemViewModel>)Tickets;

                        foreach(var t in x)
                            tickets.Add(t);
                    });
            refreshTickets.Execute(null);
        }

        public IReactiveCommand GoBack { get; private set; }

        public IReactiveList<TicketItemViewModel> Tickets { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment { get { return "myTickets"; } }
    }
}
