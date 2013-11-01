﻿using AdmitOne.Domain;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AdmitOne.Persistence
{
    public class TicketingRepository : Repository<TicketingContext>
    {
        public TicketingRepository() : base(new TicketingContext()) { }
    }

    internal sealed class TicketSysDbConfiguration : DbConfiguration
    {
        public TicketSysDbConfiguration()
        {
            SetDefaultConnectionFactory(new SqlConnectionFactory());
        }
    }

    public sealed class TicketingContext : DbContext
    {
        public IDbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Make all table names singular by default.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Make all string columns required by default.
            modelBuilder.Properties<string>()
                .Configure(x => x.IsRequired());

            base.OnModelCreating(modelBuilder);
        }
    }
}