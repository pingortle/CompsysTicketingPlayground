using System;
using System.Collections.Generic;

namespace AdmitOne.Domain.Entities
{
    public interface IEntity
    {
        int Id { get; }
    }

    public interface ICustomer : IEntity
    {
        string BusinessName { get; }
        string ContactName { get; }
        string Phone { get; }
    }

    public interface IEmployee : IEntity
    {
        string Name { get;}
    }

    public interface ITicket : IEntity
    {
        string Description { get; }

        int CustomerId { get; }
    }

    public interface ITicketEvent : IEntity
    {
        TicketStatus? TicketStatus { get; }
        DateTime Time { get; }

        int EmployeeId { get; }

        int TicketId { get; }
    }
}
