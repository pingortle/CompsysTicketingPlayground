using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Persistence
{
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
