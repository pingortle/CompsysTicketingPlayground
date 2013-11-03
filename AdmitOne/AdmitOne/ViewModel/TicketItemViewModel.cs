using ReactiveUI;
using System;

namespace AdmitOne.ViewModel
{
    public class TicketItemViewModel: ReactiveObject
    {
        public TicketItemViewModel(string text, Action<object> removeAction = null)
        {
            removeAction = removeAction ?? (x => Console.WriteLine("NoOp."));
            Text = text;

            RemoveItem = new ReactiveCommand();
            RemoveItem.Subscribe(removeAction);
        }

        public string Text { get; private set; }

        public IReactiveCommand RemoveItem { get; private set; }
    }
}
