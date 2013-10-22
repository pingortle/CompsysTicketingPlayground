using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdmitOne.ViewModel
{
    class DefaultScreen : IScreen
    {
        private IDependencyResolver dependencyResolver;

        public DefaultScreen(IDependencyResolver dependencyResolver)
        {
            throw new NotImplementedException("Default screen has not been implemented yet.");

            this.dependencyResolver = dependencyResolver;
        }

        public IRoutingState Router
        {
            get { throw new NotImplementedException(); }
        }
    }
}
