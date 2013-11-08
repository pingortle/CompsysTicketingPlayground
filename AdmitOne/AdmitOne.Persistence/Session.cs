using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;

namespace AdmitOne.Persistence
{
    public class Session : ISession
    {
        private DbContext _context;
        private Subject<bool> _isProcessing = new Subject<bool>();
        private IDisposable _workSubscription;
        private ISubject<Action, Action> _subjWork = Subject.Synchronize(new Subject<Action>());
        private ISubject<Exception> _exceptions;

        internal Session(DbContext context)
        {
            _context = context;

            _workSubscription = _subjWork
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(x => x());

            IsWorking = _isProcessing
                .Publish(false)
                .PermaRef()
                .DistinctUntilChanged();

            ThrownExceptions = _exceptions = new Subject<Exception>();
        }

        public IStore<TStore> GetStoreOf<TStore>()
        {
            return new Store<TStore>(_context);
        }

        public IEnumerable<Type> GetAvailableTypes()
        {
            // Get all of the public, instance properties
            return _context.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)

                // Filter by...
                .Where(p =>

                    // Restricting to generically typed properties
                     p.PropertyType.IsGenericType &&

                        // Checking if the property is an IDbSet<> itself, or...
                        ((p.PropertyType.IsInterface && p.PropertyType.GetGenericTypeDefinition() == typeof(IDbSet<>)) ||

                        // Checking if the property implements IDbSet<>
                        (p.PropertyType.GetInterfaces()
                            .Where(m => m.IsGenericType)
                            .Select(m => m.GetGenericTypeDefinition())
                            .Any(m => m == typeof(IDbSet<>)))))

                // Then dump all of the resulting type arguments into one Enumerable
                .SelectMany(x => x.PropertyType.GetGenericArguments());
        }

        public IObservable<T> FetchResults<T>()
        {
            return FetchResults<T, T>(new Query<T>());
        }

        public IObservable<TResult> FetchResults<TSource, TResult>(IQuery<TSource, TResult> query)
        {
            var subj = new Subject<TResult>();
            _subjWork.OnNext(() =>
            {
                lock (_isProcessing) _isProcessing.OnNext(true);
                try
                {
                    query.Against(GetStoreOf<TSource>())
                        .ToObservable()
                        .Finally(() =>
                        {
                            lock (_isProcessing) _isProcessing.OnNext(false);
                        })
                        .Subscribe(subj);
                }
                catch (Exception ex)
                {
                    _exceptions.OnNext(ex);
                }
            });

            return subj;
        }

        public INotifyWhenComplete ScopedChanges()
        {
            return new ScopeChanges(_context, _subjWork, _isProcessing, _exceptions);
        }

        public IObservable<bool> IsWorking { get; private set; }
        public IObservable<Exception> ThrownExceptions { get; private set; }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();

            _workSubscription.Dispose();
        }

        private sealed class ScopeChanges : INotifyWhenComplete
        {
            public ScopeChanges(DbContext context, ISubject<Action, Action> workQueue, ISubject<bool> isProcessing, ISubject<Exception> exceptions)
            {
                _context = context;
                _workQueue = workQueue;
                _isProcessing = isProcessing;
                _exceptions = exceptions;
                _completion = new Subject<bool>();
            }

            public IObservable<bool> Completion
            {
                get { return _completion; }
            }

            public void Dispose()
            {
                if (_workQueue != null)
                    _workQueue.OnNext(() =>
                    {
                        lock (_isProcessing) { _isProcessing.OnNext(true); }
                        try
                        {
                            _context.SaveChanges();
                            _completion.OnNext(true);
                        }
                        catch (Exception ex)
                        {
                            _completion.OnNext(false);
                            _exceptions.OnNext(ex);
                        }

                        _completion.OnCompleted();
                        lock (_isProcessing) { _isProcessing.OnNext(false); }
                    });
            }

            private DbContext _context;
            private ISubject<Action, Action> _workQueue;
            private ISubject<bool> _isProcessing;
            private Subject<bool> _completion;
            private ISubject<Exception> _exceptions;
        }
    }

    // From ReactivUI
    internal static class PermaRefMixin
    {
        internal static IObservable<T> PermaRef<T>(this IConnectableObservable<T> This)
        {
            This.Connect();
            return This;
        }
    }
}
