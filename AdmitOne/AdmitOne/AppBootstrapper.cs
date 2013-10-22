using AdmitOne.View;
using AdmitOne.ViewModel;
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
        public AppBootstrapper() : this(null, null) {}

        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, IRoutingState routingState = null)
        {
           Router = routingState ?? new RoutingState();
            dependencyResolver = dependencyResolver ?? RxApp.MutableResolver;

            RegisterDependencies(dependencyResolver);

            LogHost.Default.Level = LogLevel.Debug;

            LoginWidgetViewModel = new LoginWidgetViewModel(dependencyResolver.GetService<ILogPeopleIn>());

            Router.Navigate.Execute(new MainViewModel(dependencyResolver.GetService<IScreen>()));
        }

        public LoginWidgetViewModel LoginWidgetViewModel { get; private set; }

        #region Implements IScreen
        public IRoutingState Router { get; private set; }
        #endregion

        #region Private Configuration Logic
        // This is where we configure the composition of our components.
        private void RegisterDependencies(IMutableDependencyResolver dr)
        {
            //  Singletons
            dr.RegisterConstant(this, typeof(IScreen));
            dr.RegisterLazySingleton(() => new LoginManager(), typeof(ILogPeopleIn));
            // Views
            dr.Register(() => new MainView(), typeof(IViewFor<MainViewModel>));
            dr.Register(() => new LoginWidgetView(), typeof(IViewFor<LoginWidgetViewModel>));
        }
        #endregion
    }
}
