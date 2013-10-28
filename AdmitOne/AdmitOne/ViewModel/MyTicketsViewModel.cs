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
        public MyTicketsViewModel(IScreen screen)
        {
            HostScreen = screen;
            GoBack = HostScreen.Router.NavigateBack;

            var repo = new TicketRepository();

            Tickets = new ReactiveList<TicketItemViewModel>(repo.GetTickets().AsEnumerable().Select(x => new TicketItemViewModel(x.Description)));
        }

        public IReactiveCommand GoBack { get; private set; }

        public IReactiveList<TicketItemViewModel> Tickets { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment { get { return "myTickets"; } }
    }
}
