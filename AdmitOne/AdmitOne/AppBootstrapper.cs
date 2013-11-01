using AdmitOne.Data.Domain;
using AdmitOne.Data.Protozoa;
using AdmitOne.View;
using AdmitOne.ViewModel;
using Ninject;
using Ninject.Extensions.NamedScope;
using ReactiveUI;

namespace AdmitOne
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper() : this(null, null) { }

        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, IRoutingState routingState = null)
        {
            Router = routingState ?? new RoutingState();

            #region Ninject Setup
            var kernel = new StandardKernel();

            RxApp.InitializeCustomResolver((obj, type) => kernel.Bind(type).ToConstant(obj));

            RxApp.DependencyResolver = new FuncDependencyResolver((type, contract) => kernel.GetAll(type, contract));

            // Singletons
            kernel.Bind<IScreen>().ToConstant<AppBootstrapper>(this);
            kernel.Bind<ILogPeopleIn>().To<LoginManager>().InSingletonScope();

            // Data access
            kernel.Bind<ISee<Ticket>>().To<GenericRepository<Ticket>>().InParentScope();
            kernel.Bind<IStore<Ticket>>().To<GenericRepository<Ticket>>().InParentScope();
            kernel.Bind<IStore<Customer>>().To<GenericRepository<Customer>>().InParentScope();

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

        #region Implements IScreen
        public IRoutingState Router { get; private set; }
        #endregion
    }
}
