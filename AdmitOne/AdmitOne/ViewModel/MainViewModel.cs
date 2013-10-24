using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdmitOne.ViewModel
{
    public class MainViewModel : ReactiveObject, IRoutableViewModel
    {
        public MainViewModel(IScreen screen)
        {
            HostScreen = screen;

            NavigateToCreateTickets = screen.Router.NavigateCommandFor<CreateTicketsViewModel>();
            NavigateToDispatch = screen.Router.NavigateCommandFor<DispatchViewModel>();
            NavigateToMyTickets = HostScreen.Router.NavigateCommandFor<MyTicketsViewModel>();
        }

        public IReactiveCommand NavigateToCreateTickets { get; private set; }
        public IReactiveCommand NavigateToDispatch { get; private set; }
        public IReactiveCommand NavigateToMyTickets { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "main"; }
        }
    }
}
