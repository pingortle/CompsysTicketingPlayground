using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdmitOne
{
    public sealed class LoginWidgetViewModel : ReactiveObject
    {
        private ILogPeopleIn _loginManager;

        public LoginWidgetViewModel(ILogPeopleIn loginManager)
        {
            // TODO: Complete member initialization
            _loginManager = loginManager;
        }
    }
}
