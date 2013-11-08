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

        private Func<IQueryable<T>, IQueryable<T>> _applyQuery;
    }
}
