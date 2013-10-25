using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Data
{
    public interface ITable<T> : IQueryable<T>, IEnumerable<T>
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);
        void Attach(T item);
    }

    public interface IUnitOfWork<T>
    {
        ITable<T> Items { get; }
        void SaveChanges();
    }

    public interface IRepository<T> : IUnitOfWork<T>
    {
        IEnumerable<T> FindAll();
    }
}
