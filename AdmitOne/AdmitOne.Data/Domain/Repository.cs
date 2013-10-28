//using System;
//using System.Collections.Generic;
//using System.Data.Linq;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AdmitOne.Data.Domain
//{
//    public class Repository
//    {
//        public Repository() : this(null) { throw new NotImplementedException(); }
//        public Repository(IUnitOfWork uow)
//        {
//            _uow = uow;
//        }

//        public Customer FindCustomer(int id)
//        {
//            return (from c in Customers
//                    where c.Id == id
//                    select c).SingleOrDefault<Customer>();
//        }

//        public IQueryable<Customer> Customers
//        {
//            get { return _uow.Items.Cast<Customer>(); }
//        }

//        private IUnitOfWork _uow;
//    }

//    public class GenericRepository<T> : IRepository<T>
//    {
//        public GenericRepository(IUnitOfWork<T> uow)
//        {
//            _uow = uow;
//        }

//        public void Add(T item)
//        {
//            _uow.Items.Add(item);
//        }

//        public void Remove(T item)
//        {
//            _uow.Items.Remove(item);
//        }

//        public void Update(T item)
//        {
//            _uow.Items.Update(item);
//        }

//        public IQueryable<T> GetItems()
//        {
//            return _uow.Items;
//        }

//        private IUnitOfWork<T> _uow;
//    }

//    public class UnitOfWork<T> : IUnitOfWork<T>
//    {
//        public UnitOfWork(DataContext dc)
//        {
//            _dc = dc;
//            _table = new EntityTable<T>(_dc.GetTable(typeof(T)));
//        }

//        public ITable<T> Items
//        {
//            get { return _table; }
//        }

//        public void SaveChanges()
//        {
//            _dc.SubmitChanges();
//        }

//        private DataContext _dc;
//        private ITable<T> _table;
//    }

//    public class FakeTable<T> : ITable<T>
//    {
//        private IList<T> _items;

//        public FakeTable(IList<T> source)
//        {
//            _items = source;
//        }

//        public IEnumerator<T> GetEnumerator()
//        {
//            return _items.GetEnumerator();
//        }

//        public Type ElementType
//        {
//            get { return typeof(T); }
//        }

//        public System.Linq.Expressions.Expression Expression
//        {
//            get { return _items.AsQueryable().Expression; }
//        }

//        public IQueryProvider Provider
//        {
//            get { return _items.AsQueryable().Provider; }
//        }

//        public int IndexOf(T item)
//        {
//            return _items.IndexOf(item);
//        }

//        public void Insert(int index, T item)
//        {
//            _items.Insert(index, item);
//        }

//        public void RemoveAt(int index)
//        {
//            _items.RemoveAt(index);
//        }

//        public T this[int index]
//        {
//            get
//            {
//                return _items[index];
//            }
//            set
//            {
//                _items[index] = value;
//            }
//        }

//        public void Add(T item)
//        {
//            _items.Add(item);
//        }

//        public void Clear()
//        {
//            _items.Clear();
//        }

//        public bool Contains(T item)
//        {
//            return _items.Contains(item);
//        }

//        public void CopyTo(T[] array, int arrayIndex)
//        {
//            _items.CopyTo(array, arrayIndex);
//        }

//        public int Count
//        {
//            get { return _items.Count; }
//        }

//        public bool IsReadOnly
//        {
//            get { return _items.IsReadOnly; }
//        }

//        public bool Remove(T item)
//        {
//            return _items.Remove(item);
//        }


//        void ITable<T>.Remove(T item)
//        {
//            _items.Remove(item);
//        }

//        public void Update(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Attach(T item)
//        {
//            throw new NotImplementedException();
//        }

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class EntityTable<T> : ITable<T>
//    {
//        private Table<T> _table;

//        public EntityTable(Table<T> table)
//        {
//            throw new NotImplementedException();

//            _table = table;
//        }

//        public void Add(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Remove(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Update(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Attach(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerator<T> GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }

//        public Type ElementType
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public System.Linq.Expressions.Expression Expression
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public IQueryProvider Provider
//        {
//            get { throw new NotImplementedException(); }
//        }
//    }
//}
