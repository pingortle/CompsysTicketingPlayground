using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdmitOne.ViewModel
{
    public sealed class DispatchViewModel : ReactiveObject, IRoutableViewModel
    {
        public DispatchViewModel(IScreen screen)
        {
            HostScreen = screen;
            GoBack = HostScreen.Router.NavigateBack;
        }

#warning Demo code!
        public IEnumerable<string> Techs { get { return new[] { "Tech1", "Tech2", "Tech2", "Tech2", "Tech2", "Tech2", "Tech2", "Tech2", }; } }
        public IEnumerable<string> Tickets { get { return new[] { "Ticket1", "Ticket2", "Ticket2", "Ticket2", "Ticket2", "Ticket2", "Ticket2", "Ticket2", }; } }

        public IReactiveCommand GoBack { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment { get { return "dispatch"; } }
    }
}
