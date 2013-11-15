using AdmitOne.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AdmitOne.Persistence.Ticketing
{
    public class TicketingSession : Session
    {
        public TicketingSession() : base(new TicketingContext()) { }
    }

    internal sealed class TicketSysDbConfiguration : DbConfiguration
    {
        public TicketSysDbConfiguration()
        {
            SetDefaultConnectionFactory(new SqlConnectionFactory());
        }
    }

    internal sealed class TicketingContextInitializer : DropCreateDatabaseIfModelChanges<TicketingContext>
    {
        protected override void Seed(TicketingContext context)
        {
            foreach (var c in new Customer[]
            {
                new Customer
                {
                    Id = 1,
                    BusinessName = "KFC",
                    ContactName = "The Colonel",
                    Phone = "1234567",
                    Tickets = new List<Ticket>
                    {
                        new Ticket { Id = 1, Description = "Eviscerated chickens." },
                        new Ticket { Id = 2, Description = "Bock-ilism poisoning." },
                    },
                },
                new Customer
                {
                    Id = 2,
                    BusinessName = "Compsys",
                    ContactName = "Kaleb",
                    Phone = "7654321",
                    Tickets = new List<Ticket>
                    {
                        new Ticket { Id = 3, Description = "Computers broke." },
                    },
                },
                new Customer
                {
                    Id = 3,
                    BusinessName = "Govmnt",
                    ContactName = "Obama",
                    Phone = "(xxx) xxx xxxx",
                    Tickets = new List<Ticket>
                    {
                        new Ticket { Id = 4, Description = "Website broken." },
                        new Ticket { Id = 5, Description = "Down in polls." },
                        new Ticket { Id = 6, Description = "Teleprompter keeps lying to me." },
                    },
                },
                new Customer
                {
                    Id = 4,
                    BusinessName = "Apple Inc.",
                    ContactName = "Tim Cook",
                    Phone = "timcook@icloud.com",
                    Tickets = new List<Ticket>
                    {
                        new Ticket
                        {
                            Id = 7,
                            Description = "iOS 7 is hurting my eyes.",
                            TicketNotes = new List<TicketNote> { new TicketNote { Id = 1, Note = "Too... much... pink...", Time = DateTime.Now.AddDays(-5) }, }
                        },
                    },
                },
            })
            {
                context.Customers.Add(c);
            }

            foreach (var c in new Employee[]
            {
                new Employee
                {
                    Id = 1,
                    Name = "Kaleb Lape",
                    TicketEvents = new List<TicketEvent>
                    {
                        new TicketEvent { Id = 1, TicketId = 3, TicketStatus = TicketStatus.Open, Time = DateTime.Now.AddDays(-3) },
                        new TicketEvent { Id = 2, TicketId = 3, TicketStatus = TicketStatus.Assigned, Time = DateTime.Now.AddDays(-1) },
                        new TicketEvent { Id = 3, TicketId = 2, TicketStatus = TicketStatus.Closed, Time = DateTime.Now },
                    },
                },
                new Employee
                {
                    Id = 2,
                    Name = "Joe Blow",
                    TicketEvents = new List<TicketEvent>
                    {
                        new TicketEvent { Id = 4, TicketId = 2, TicketStatus = TicketStatus.Open, Time = DateTime.Now.AddDays(-4) },
                    },
                },
                new Employee
                {
                    Id = 3,
                    Name = "Some Dude",
                    TicketEvents = new List<TicketEvent>
                    {
                        new TicketEvent { Id = 5, TicketId = 7, TicketStatus = TicketStatus.Open, Time = DateTime.Now },
                        new TicketEvent { Id = 6, TicketId = 6, TicketStatus = TicketStatus.Open, Time = DateTime.Now },
                        new TicketEvent { Id = 7, TicketId = 5, TicketStatus = TicketStatus.Open, Time = DateTime.Now },
                        new TicketEvent { Id = 8, TicketId = 4, TicketStatus = TicketStatus.Open, Time = DateTime.Now },
                    },
                },
                new Employee
                {
                    Id = 3,
                    Name = "Time Traveller",
                    TicketEvents = new List<TicketEvent>
                    {
                        new TicketEvent { Id = 5, TicketId = 5, TicketStatus = TicketStatus.Assigned, Time = DateTime.Now.AddYears(100) },
                    },
                },
            })
            {
                context.Employees.Add(c);
            }

            context.SaveChanges();
        }
    }

    public sealed class TicketingContext : DbContext
    {
        public TicketingContext() : base() { }
        public TicketingContext(string connectionOrName) : base(connectionOrName) { }

        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Properties<string>()
                .Configure(x => x.IsRequired());

            base.OnModelCreating(modelBuilder);
        }

        static TicketingContext()
        {
            Database.SetInitializer(new TicketingContextInitializer());
        }
    }
}
