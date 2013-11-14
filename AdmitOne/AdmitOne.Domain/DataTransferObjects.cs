using AdmitOne.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AdmitOne.Domain
{
    public class TicketWithEvent : ITicket, ITicketEvent
    {
        #region Generated Code
        public string Description
        {
            get; set;
        }

        public int CustomerId
        {
            get; set;
        }

        public int Id
        {
            get; set;
        }

        public TicketStatus? TicketStatus
        {
            get; set;
        }

        public DateTime Time
        {
            get; set;
        }

        public int EmployeeId
        {
            get; set;
        }

        public int TicketId
        {
            get; set;
        }
        #endregion
    }
}
