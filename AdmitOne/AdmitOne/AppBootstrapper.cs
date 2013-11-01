﻿using AdmitOne.View;
using AdmitOne.ViewModel;
using Ninject;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        // Necessary for construction from App.xaml
        public AppBootstrapper() : this(null, null) { }

        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, IRoutingState routingState = null)
        {
            #region Handle Optional Parameters
            Router = routingState ?? new RoutingState();

            var kernel = new StandardKernel();

            RxApp.DependencyResolver = dependencyResolver ?? new FuncDependencyResolver((type, contract) => kernel.GetAll(type, contract));

            if (dependencyResolver == null)
                RxApp.InitializeCustomResolver((obj, type) => kernel.Bind(type).ToConstant(obj));
            #endregion

            #region Ninject Setup
            // Singletons
            kernel.Bind<IScreen>().ToConstant<AppBootstrapper>(this);
            kernel.Bind<ILogPeopleIn>().To<LoginManager>().InSingletonScope();

            // View resolution
            kernel.Bind<IViewFor<LoginWidgetViewModel>>().To<LoginWidgetView>();
            kernel.Bind<IViewFor<MainViewModel>>().To<MainView>();
            kernel.Bind<IViewFor<CreateTicketsViewModel>>().To<CreateTicketsView>();
            kernel.Bind<IViewFor<DispatchViewModel>>().To<DispatchView>();
            kernel.Bind<IViewFor<MyTicketsViewModel>>().To<MyTicketsView>();
            #endregion

            LogHost.Default.Level = LogLevel.Debug;

            LoginWidgetViewModel = kernel.Get<LoginWidgetViewModel>();

            Router.Navigate.Execute(kernel.Get<MainViewModel>());
        }

        public LoginWidgetViewModel LoginWidgetViewModel { get; private set; }

        #region IScreen
        public IRoutingState Router { get; private set; }
        #endregion
    }
}
