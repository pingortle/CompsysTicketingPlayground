using AdmitOne.Domain;
using AdmitOne.Persistence;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace AdmitOne.ViewModel
{
    public sealed class MyTicketsViewModel : ReactiveObject, IRoutableViewModel
    {
        public MyTicketsViewModel(IScreen screen, IRepository repository)
        {
            HostScreen = screen;
            GoBack = HostScreen.Router.NavigateBack;

            Tickets = new ReactiveList<Ticket>();
            
            var repositoryObservable = Observable.Defer(() =>
                repository.GetStoreOf<Ticket>().ToObservable())
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler);

            var published = Observable.Publish(repositoryObservable);
            published.Connect();

            published
                .Subscribe(x =>
                    {
                        Tickets.Add(x);
                    });

            _isFetchingTickets = published
                .Select(_ => false)
                .ToProperty(this, x => x.IsFetchingTickets, true);
        }

        private ObservableAsPropertyHelper<bool> _isFetchingTickets;
        public bool IsFetchingTickets { get { return _isFetchingTickets.Value; } }

        public IReactiveCollection<Ticket> Tickets { get; set; }

        public IReactiveCommand GoBack { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment { get { return "myTickets"; } }
    }
}
