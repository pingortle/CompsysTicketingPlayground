using AdmitOne.Domain.Entities;
using ReactiveUI;
using System;

namespace AdmitOne.ViewModel
{
    public class TicketItemViewModel: ReactiveObject
    {
        public TicketItemViewModel(string description, Action<object> removeAction)
            : this(description, TicketStatus.Open, removeAction)
        { }

        public TicketItemViewModel(string description, TicketStatus status = TicketStatus.Open, Action<object> removeAction = null)
        {
            removeAction = removeAction ?? (x => Console.WriteLine("NoOp."));
            Description = description;
            Status = status;

            RemoveItem = new ReactiveCommand();
            RemoveItem.Subscribe(removeAction);
        }

        public string Description { get; private set; }

        private TicketStatus _status;
        public TicketStatus Status
        {
            get { return _status; }
            set { this.RaiseAndSetIfChanged(ref _status, value); }
        }

        public IReactiveCommand RemoveItem { get; private set; }
    }
}
