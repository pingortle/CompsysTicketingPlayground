using AdmitOne.Domain;
using AdmitOne.Persistence;
using ReactiveUI;
using System.Linq;
using System.Reactive.Linq;
using System;

namespace AdmitOne.ViewModel
{
    public sealed class MyTicketsViewModel : ReactiveObject, IRoutableViewModel
    {
        public MyTicketsViewModel(IScreen screen, ISession session)
        {
            HostScreen = screen;
            GoBack = HostScreen.Router.NavigateBack;

            Tickets = new ReactiveList<Ticket>();

            _isFetchingTickets = session.IsWorking
                .ToProperty(this, x => x.IsFetchingTickets);

            session.FetchResults(new Query<Ticket>())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => Tickets.Add(x));
        }

        private ObservableAsPropertyHelper<bool> _isFetchingTickets;
        public bool IsFetchingTickets { get { return _isFetchingTickets.Value; } }

        public IReactiveCollection<Ticket> Tickets { get; set; }

        public IReactiveCommand GoBack { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment { get { return "myTickets"; } }
    }
}
