﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        internal Session(DbContext context)
        {
            _context = context;

            _workSubscription = _subjWork
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(x => x());

            IsFetchingResults = _isProcessing
                .Publish(false)
                .PermaRef()
                .DistinctUntilChanged();
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

        public IObservable<T> FetchResults<T>(IQuery<T> query)
        {
            var subj = new Subject<T>();
            _subjWork.OnNext(() =>
            {
                lock (_isProcessing) _isProcessing.OnNext(true);
                query.Against(GetStoreOf<T>())
                    .ToObservable()
                    .Finally(() =>
                    {
                        subj.OnCompleted();
                        lock (_isProcessing) _isProcessing.OnNext(false);
                    })
                    .Subscribe(subj);
            });

            return subj;
        }

        public IDisposable ScopedChanges()
        {
            return new ScopeChanges(_context, _subjWork, _isProcessing);
        }

        public IObservable<bool> IsFetchingResults { get; private set; }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();

            _workSubscription.Dispose();
        }

        private sealed class ScopeChanges : IDisposable
        {
            public ScopeChanges(DbContext context, ISubject<Action, Action> workQueue, ISubject<bool> isProcessing)
            {
                _context = context;
                _workQueue = workQueue;
                _isProcessing = isProcessing;
            }

            public void Dispose()
            {
                if (_workQueue != null)
                    _workQueue.OnNext(() =>
                    {
                        lock (_isProcessing) { _isProcessing.OnNext(true); }
                        _context.SaveChanges();
                        lock (_isProcessing) { _isProcessing.OnNext(false); }
                    });
            }

            private DbContext _context;
            private ISubject<Action, Action> _workQueue;
            private ISubject<bool> _isProcessing;
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
