using ReactiveUI;
using System;
using System.Collections;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace AdmitOne.ViewModel
{
    public class CreateTicketsViewModel : ReactiveObject, IRoutableViewModel
    {
#warning Demonstration purposes only!
        static Random _randomThisShouldNotBeHere = new Random(0);

        public CreateTicketsViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? new DefaultScreen(RxApp.DependencyResolver);

            GoBack = HostScreen.Router.NavigateBack;

            CurrentBatch = new ReactiveList<TicketItemViewModel>();

            AddTicket = new ReactiveCommand(
                this.WhenAny(
                x => x.CurrentBatch,
                y => y.TicketText,
                (x, y) => x.Value.Count() <= 100 && !string.IsNullOrWhiteSpace(y.Value)));
            AddTicket.Subscribe(_ =>
                {
                    CurrentBatch.Add(new TicketItemViewModel(TicketText, CurrentBatch.Remove));
                    TicketText = default(string);
                });

            var anyInList = CurrentBatch.Changed.Select(_ => CurrentBatch.Any());

            SaveChanges = new ReactiveCommand(anyInList.StartWith(false));

            _isExecuting = SaveChanges.IsExecuting.ToProperty(this, x => x.IsExecuting, false);

            SaveChanges.RegisterAsyncFunction(x =>
            {
#warning Demonstration purposes only!
                // Do some work which may or may not fail.
                System.Threading.Thread.Sleep(3000);

                // Return the success status of the work.
                return _randomThisShouldNotBeHere.Next(0, 2) == 1;
            })
            .Subscribe(x =>
                    {
                        if (x)
                            (CurrentBatch as IList).Clear();
                    });
        }

        private ObservableAsPropertyHelper<bool> _isExecuting;
        public bool IsExecuting { get { return _isExecuting.Value; } }

        public IReactiveCommand AddTicket { get; private set; }

        public IReactiveCommand SaveChanges { get; private set; }

        private string _ticketText;
        public string TicketText
        {
            get { return _ticketText; }
            set { this.RaiseAndSetIfChanged(ref _ticketText, value); }
        }

        public IReactiveList<TicketItemViewModel> CurrentBatch { get; private set; }

        public IReactiveCommand GoBack { get; private set; }

        public IScreen HostScreen { get; private set; }

        public string UrlPathSegment
        {
            get { return "create_tickets"; }
        }
    }
}
