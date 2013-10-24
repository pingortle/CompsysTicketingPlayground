using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Data.Domain
{
    public class Repository
    {
        public Repository() { }
        public Repository(DataContext context)
        {
            Customers = context.GetTable<Customer>();
        }

        public Customer FindCustomer(string id)
        {
            return (from c in Customers
                    where c.ID == id
                    select c).SingleOrDefault<Customer>();
        }

        private IQueryable<Customer> _customers;
        public IQueryable<Customer> Customers
        {
            get { return _customers; }
            set { _customers = value; }
        }
    }
}
