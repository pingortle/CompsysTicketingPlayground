using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

namespace AdmitOne.Persistence
{
    public interface ISession : IDisposable
    {
        IStore<T> GetStoreOf<T>();
        IEnumerable<Type> GetAvailableTypes();

        INotifyWhenComplete ScopedChanges();

        IObservable<T> FetchResults<T>();
        IObservable<T> FetchResults<T>(IQuery<T> query);
        IObservable<bool> IsWorking { get; }
    }

    public interface ISee<out T> : IQueryable<T>, IEnumerable<T> { }

    public interface ITake<in T>
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);
        void Attach(T item);
    }

    public interface IStore<in T1, out T2> : ITake<T1>, ISee<T2>
    {}

    public interface IStore<T> : IStore<T, T> { }

    public interface IQuery<T>
    {
        IEnumerable<T> Against(IQueryable<T> source);
    }

    public interface INotifyWhenComplete : IDisposable
    {
        IObservable<bool> Completion { get; }
    }
}
