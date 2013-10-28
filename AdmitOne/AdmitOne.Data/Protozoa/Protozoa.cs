using AdmitOne.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Data.Protozoa
{
    public class TicketRepository
    {
        DataContext _dc;
        Table<Ticket> _table;

        public TicketRepository()
        {
            var context = new AdmitOneContext();
            _table = context.GetTable<Ticket>();
            _dc = context;
        }

        private Subject<Exception> _thrownExceptions = new Subject<Exception>();
        public IObservable<Exception> ThrownExceptions { get { return _thrownExceptions; } }

        public void AddTicket(Ticket ticket)
        {
            _table.InsertOnSubmit(ticket);
        }

        public IQueryable<Ticket> GetTickets()
        {
            return _dc.GetTable<Ticket>();
        }

        public void SaveChanges()
        {
            try
            {
                _dc.SubmitChanges();
            }
            catch (Exception e)
            {
                _thrownExceptions.OnNext(e);
            }
        }
    }
}
