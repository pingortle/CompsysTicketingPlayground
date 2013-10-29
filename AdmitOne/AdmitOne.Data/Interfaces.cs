using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Data
{
    public interface ITable : IQueryable, System.Collections.IEnumerable
    {
        void Add(object item);
        void Remove(object item);
        void Update(object item);
        void Attach(object item);
    }

    public interface ITable<T> : IQueryable<T>, IEnumerable<T>, IEnumerable, IQueryable
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);
        void Attach(T item);
    }

    public interface IUnitOfWork
    {
        ITable Items { get; }
        void SaveChanges();
    }

    public interface IUnitOfWork<T>
    {
        ITable<T> Items { get; }
        void SaveChanges();
    }

    public interface IRepository<T>
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);

        IQueryable<T> GetItems();
    }
}
