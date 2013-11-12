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

        List<Ticket> Tickets { get; }
    }

    public interface IEmployee : IEntity
    {
        string Name { get;}

        List<TicketEvent> TicketEvents { get; }
    }

    public interface ITicket : IEntity
    {
        string Description { get; }

        int CustomerId { get; }
        Customer Customer { get; }

        List<TicketEvent> TicketEvents { get; }
    }

    public interface ITicketEvent : IEntity
    {
        TicketStatus? TicketStatus { get; }
        DateTime Time { get; }

        int EmployeeId { get; }
        Employee Employee { get; }

        int TicketId { get; }
        Ticket Ticket { get; }
    }
}
