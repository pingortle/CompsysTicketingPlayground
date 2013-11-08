using AdmitOne.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace AdmitOne.Persistence
{
    public class TicketQuery : IQuery<Ticket>
    {
        public TicketQuery()
        {
            _fakeSource = new FakeQuerySource<Ticket>();
            Queryable = _fakeSource;
        }

        protected IQueryable<Ticket> Queryable { get; private set; }

        public IQuery<Ticket> With(IQuery<Ticket> query)
        {
            return new TicketQuery();
        }

        public IEnumerable<Ticket> Against(IQueryable<Ticket> source)
        {
            using (_fakeSource.ScopedSwap(source))
            {
                return Queryable.ToList();
            }
        }

        private FakeQuerySource<Ticket> _fakeSource;

        private class FakeQuerySource<T> : IQueryable<T>
        {
            public IDisposable ScopedSwap(IQueryable<T> swapQueryable)
            {
                _source = swapQueryable;
                return Disposable.Create(() => _source = null);
            }

            public IEnumerator<T> GetEnumerator()
            {
                VerifySource();
                return _source.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                VerifySource();
                return _source.GetEnumerator();
            }

            public Type ElementType
            {
                get
                {
                    VerifySource();
                    return _source.ElementType;
                }
            }

            public System.Linq.Expressions.Expression Expression
            {
                get
                {
                    VerifySource();
                    return _source.Expression;
                }
            }

            public IQueryProvider Provider
            {
                get
                {
                    VerifySource();
                    return _source.Provider;
                }
            }

            private void VerifySource()
            {
                if (_source == null) throw new InvalidOperationException("Must be backed by a real query source before executing a query.");
            }

            private IQueryable<T> _source;
        }
    }
}
