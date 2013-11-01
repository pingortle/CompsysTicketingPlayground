using AdmitOne.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Reactive.Subjects;

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

    public class TicketRepository2 : IStore<Ticket>
    {
        DataContext _dc;
        Table<Ticket> _table;

        public TicketRepository2()
        {
            var context = new AdmitOneContext();
            _table = context.GetTable<Ticket>();
            _dc = context;
        }

        public void Add(Ticket item)
        {
            _table.InsertOnSubmit(item);
            _dc.SubmitChanges();
        }

        public void Remove(Ticket item)
        {
            _table.DeleteOnSubmit(item);
        }

        public void Update(Ticket item)
        {
            var original = _table.First(x => x.Id == item.Id);
            _table.Attach(item, original);
        }

        public void Attach(Ticket item)
        {
            _table.Attach(item);
        }

        public IEnumerator<Ticket> GetEnumerator()
        {
            return _table.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _table.GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(Ticket); }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return ((IQueryable)_table).Expression; }
        }

        public IQueryProvider Provider
        {
            get { return ((IQueryable)_table).Provider; }
        }
    }

    public class GenericRepository<T> : IStore<T>
    {
        System.Data.Linq.ITable _table;

        public GenericRepository()
        {
            var context = new AdmitOneContext();
            _table = (System.Data.Linq.ITable)context.GetTable(typeof(T));
        }

        public void Add(T item)
        {
            _table.InsertOnSubmit(item);
            _table.Context.SubmitChanges();
        }

        public void Remove(T item)
        {
            _table.DeleteOnSubmit(item);
        }

        public void Update(T item)
        {
            _table.Attach(item);
            _table.Context.SubmitChanges();
        }

        public void Attach(T item)
        {
            _table.Attach(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _table.Cast<T>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _table.GetEnumerator();
        }

        public Type ElementType
        {
            get { return _table.AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return _table.AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _table.AsQueryable().Provider; }
        }
    }
}
