using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Persistence
{
    public class Query<TSource, TResult> : IQuery<TSource, TResult>
    {
        public Query(Func<IQueryable<TSource>, IQueryable<TResult>> applyQuery)
        {
            ApplyQuery = applyQuery ?? (x => { throw new InvalidOperationException("Query must have a query function."); });
        }

        public IEnumerable<TResult> Against(IQueryable<TSource> source)
        {
            return ApplyQuery(source).ToList();
        }

        public IQuery<TSource, TOut> With<TOut>(Func<IQueryable<TResult>, IQueryable<TOut>> query)
        {
            return new Query<TSource, TOut>(x => query(ApplyQuery(x)));
        }

        protected Func<IQueryable<TSource>, IQueryable<TResult>> ApplyQuery;
    }

    public class Query<T> : Query<T, T>, IQuery<T>
    {
        public Query(Func<IQueryable<T>, IQueryable<T>> applyQuery = null)
            : base(null)
        {
            ApplyQuery = applyQuery ?? (q => q);
        }

        public IQuery<T> With(Func<IQueryable<T>, IQueryable<T>> query)
        {
            return new Query<T>(x => query(ApplyQuery(x)));
        }
    }
}
