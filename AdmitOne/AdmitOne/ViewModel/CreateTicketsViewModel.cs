using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdmitOne.ViewModel
{
    public class CreateTicketsViewModel : ReactiveObject, IRoutableViewModel
    {
        public CreateTicketsViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? new DefaultScreen(RxApp.DependencyResolver);

            GoBack = HostScreen.Router.NavigateBack;
        }

        public IReactiveCommand GoBack { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "create_tickets"; }
        }
    }
}
