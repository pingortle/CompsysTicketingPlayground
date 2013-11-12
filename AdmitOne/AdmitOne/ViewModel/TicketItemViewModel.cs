using AdmitOne.Domain.Entities;
using ReactiveUI;
using System;

namespace AdmitOne.ViewModel
{
    public class TicketItemViewModel: ReactiveObject
    {
        public TicketItemViewModel(string text, Action<object> removeAction)
            : this(text, TicketStatus.Open, removeAction)
        { }

        public TicketItemViewModel(string text, TicketStatus status = TicketStatus.Open, Action<object> removeAction = null)
        {
            removeAction = removeAction ?? (x => Console.WriteLine("NoOp."));
            Text = text;
            Status = status;

            RemoveItem = new ReactiveCommand();
            RemoveItem.Subscribe(removeAction);
        }

        public string Text { get; private set; }

        private TicketStatus _status;
        public TicketStatus Status
        {
            get { return _status; }
            set { this.RaiseAndSetIfChanged(ref _status, value); }
        }

        public IReactiveCommand RemoveItem { get; private set; }
    }
}
