﻿using System;
using System.Collections.Generic;

namespace AdmitOne.Domain.Entities
{
    public enum TicketStatus { Open, Closed, Assigned }

    public class Customer : ICustomer
    {
        public int Id { get; set; }

        public string BusinessName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }

        public virtual List<Ticket> Tickets { get; set; }
    }

    public class Employee : IEmployee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<TicketEvent> TicketEvents { get; set; }
    }

    public class Ticket : ITicket
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual List<TicketEvent> TicketEvents { get; set; }
        public virtual List<TicketNote> TicketNotes { get; set; }
    }

    public class TicketEvent : ITicketEvent
    {
        public int Id { get; set; }

        public TicketStatus? TicketStatus { get; set; }
        public DateTime Time { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }

    public class TicketNote : ITicketNote
    {
        public int Id { get; set; }

        public string Note { get; set; }
        public DateTime Time { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
