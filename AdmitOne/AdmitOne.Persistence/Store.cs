using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AdmitOne.Persistence
{
    internal sealed class Store<T> : IStore<T>
    {
        private DbContext _context;
        private DbSet _dbSet;

        internal Store(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set(typeof(T));
        }

        private sealed class ScopeChanges : IDisposable
        {
            public ScopeChanges(DbContext context)
            {
                _context = context;
            }

            public void Dispose()
            {
                _context.SaveChanges();
            }

            private DbContext _context;
        }

        #region IStore<>
        public void Add(T item)
        {
            _dbSet.Add(item);
        }

        public void Remove(T item)
        {
            _dbSet.Remove(item);
        }

        public void Update(T item)
        {
            _dbSet.Attach(item);
        }

        public void Attach(T item)
        {
            _dbSet.Attach(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _dbSet.AsQueryable().Cast<T>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dbSet.AsQueryable().GetEnumerator();
        }

        public Type ElementType
        {
            get { return _dbSet.AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return _dbSet.AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _dbSet.AsQueryable().Provider; }
        }

        public IDisposable ScopedChanges()
        {
            return new ScopeChanges(_context);
        }
        #endregion
    }
}
