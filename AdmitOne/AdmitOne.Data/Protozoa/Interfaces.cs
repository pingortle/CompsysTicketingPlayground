using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Data.Protozoa
{
    public interface IProvide<T>
    {
        ISee<T> All();
    }

    public interface ISee<out T> : IQueryable<T>, IEnumerable<T> { }

    public interface ITake<in T>
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);
        void Attach(T item);
    }

    public interface IStore<in T1, out T2> : ITake<T1>, ISee<T2> { }

    public interface IStore<T> : IStore<T, T> { }
}
