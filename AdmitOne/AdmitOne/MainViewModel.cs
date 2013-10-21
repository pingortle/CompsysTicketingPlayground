using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdmitOne
{
    public class MainViewModel : ReactiveObject, IRoutableViewModel
    {
        public MainViewModel(IScreen screen)
        {
            // TODO: Complete member initialization
            HostScreen = screen;
        }

        public string Text { get { return "Howdy!"; } }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "main"; }
        }
    }
}
