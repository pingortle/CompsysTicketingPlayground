using AdmitOne.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace AdmitOne.Persistence
{
    public class TicketQuery : IQuery<Ticket>
    {
        public TicketQuery(Func<IQueryable<Ticket>, IQueryable<Ticket>> applyQuery = null)
        {
            _applyQuery = applyQuery ?? (q => q);
        }

        public IEnumerable<Ticket> Against(IQueryable<Ticket> source)
        {
                return _applyQuery(source).ToList();
        }

        private Func<IQueryable<Ticket>, IQueryable<Ticket>> _applyQuery;
    }
}
