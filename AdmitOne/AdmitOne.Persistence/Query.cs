using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Persistence
{
    public class MappingQuery<TSource, TResult> : IQuery<TSource, TResult>
    {
        public MappingQuery(Func<IQueryable<TSource>, IQueryable<TResult>> applyQuery)
        {
            _applyQuery = applyQuery;
        }

        public IEnumerable<TResult> Against(IQueryable<TSource> source)
        {
            return _applyQuery(source).ToList();
        }

        public IQuery<TSource, TOut> With<TOut>(Func<IQueryable<TResult>, IQueryable<TOut>> query)
        {
            return new MappingQuery<TSource, TOut>(x => query(_applyQuery(x)));
        }

        private Func<IQueryable<TSource>, IQueryable<TResult>> _applyQuery;
    }

    public class Query<T> : IQuery<T>
    {
        public Query(Func<IQueryable<T>, IQueryable<T>> applyQuery = null)
        {
            _applyQuery = applyQuery ?? (q => q);
        }

        public IEnumerable<T> Against(IQueryable<T> source)
        {
            return _applyQuery(source).ToList();
        }

        public IQuery<T, TOut> With<TOut>(Func<IQueryable<T>, IQueryable<TOut>> query)
        {
            return new MappingQuery<T, TOut>(x => query(_applyQuery(x)));
        }

        public IQuery<T> With(Func<IQueryable<T>, IQueryable<T>> query)
        {
            return new Query<T>(x => query(_applyQuery(x)));
        }

        private Func<IQueryable<T>, IQueryable<T>> _applyQuery;
    }
}
