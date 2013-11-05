using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace AdmitOne
{
    public static class AsyncDataMixins
    {
        public static IReactiveCommand JoinMutuallyExclusiveAsyncCommands(this IEnumerable<IReactiveCommand> commands, IEnumerable<IObservable<object>> asyncResults)
        {
            if (!commands.Any() || !asyncResults.Any()) throw new ArgumentException("Enumerables must have elements.");
            if (commands.Count() != asyncResults.Count()) throw new ArgumentException("Enumerables must have equal numbers of elements.");

            var masterCommand = new ReactiveCommand(Observable.Merge(commands.Select(x => x.CanExecuteObservable)));

            masterCommand.RegisterAsyncAction(x =>
            {
                commands.Zip(asyncResults, (n, m) => new { Command = n, ObservableResult = m }).Aggregate((a, b) =>
                {
                    b.ObservableResult.Take(1).Subscribe(_ => a.Command.Execute(x));
                    return b;
                })
                    .Command.Execute(x);
            });

            return masterCommand;
        }
    }
}