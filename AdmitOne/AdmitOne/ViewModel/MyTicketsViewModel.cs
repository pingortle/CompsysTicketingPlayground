using AdmitOne.Domain;
using AdmitOne.Persistence;
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
        public MyTicketsViewModel(IScreen screen, IRepository repository)
        {
            HostScreen = screen;
            GoBack = HostScreen.Router.NavigateBack;

            Tickets = new ReactiveList<Ticket>(repository.GetStoreOf<Ticket>().OrderByDescending(x => x.Id));
        }

        public IReactiveCollection<Ticket> Tickets { get; set; }

        public IReactiveCommand GoBack { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment { get { return "myTickets"; } }
    }
}
