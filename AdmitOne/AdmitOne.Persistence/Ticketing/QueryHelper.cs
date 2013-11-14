using AdmitOne.Domain;
using AdmitOne.Domain.Entities;
using System.Linq;

namespace AdmitOne.Persistence.Ticketing
{
    public static class QueryHelper
    {
        public static readonly QueryableFunc<Ticket, TicketEvent, TicketWithEvent>
            QueryOnTicketsAndEvents = ((tickets, events) =>
                from t in tickets
                join e in events
                    .GroupBy(y => y.TicketId)
                    .SelectMany(g =>
                        g.OrderByDescending(y => y.Time).Take(1))
                on t.Id equals e.TicketId
                orderby e.Time descending
                select new TicketWithEvent
                {
                    Description = t.Description,
                    CustomerId = t.CustomerId,
                    EmployeeId = e.EmployeeId,
                    TicketId = t.Id,
                    TicketStatus = e.TicketStatus,
                    Time = e.Time
                });
    }
}
