using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Data.Domain
{
    public sealed class Customer
    {
        public Customer() { }

        public Customer(string id)
        {
            ID = id;
        }

        private IList<Ticket> _tickets = new List<Ticket>();
        public IList<Ticket> Tickets
        {
            get { return _tickets; }
            set { _tickets = value; }
        }

        public string ID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}
