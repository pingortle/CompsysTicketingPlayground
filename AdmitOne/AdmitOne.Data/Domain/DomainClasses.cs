using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdmitOne.Data.Domain
{
    public class ContractType
    {
        #region

        private ICollection<CustomerContract> _CustomerContract;
        private string _Description;
        private int _Id;
        private string _Type;


        #endregion

        #region


        public ICollection<CustomerContract> CustomerContract
        {
            get { return _CustomerContract; }
            set { _CustomerContract = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }


        #endregion
    }

    public class Customer
    {
        #region

        private ICollection<CustomerContract> _CustomerContract;
        private ICollection<CustomerLaborRate> _CustomerLaborRate;
        private string _Fax;
        private int _Id;
        private string _Name;
        private string _Phone;
        private ICollection<Ticket> _Ticket;


        #endregion

        #region


        public ICollection<CustomerContract> CustomerContract
        {
            get { return _CustomerContract; }
            set { _CustomerContract = value; }
        }

        public ICollection<CustomerLaborRate> CustomerLaborRate
        {
            get { return _CustomerLaborRate; }
            set { _CustomerLaborRate = value; }
        }

        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        public ICollection<Ticket> Ticket
        {
            get { return _Ticket; }
            set { _Ticket = value; }
        }


        #endregion
    }

    public class CustomerContract
    {
        #region

        private ContractType _ContractType;
        private Customer _Customer;
        private int _Customer_id;
        private int _Id;
        private decimal _Rate;
        private int _Type_id;


        #endregion

        #region


        public ContractType ContractType
        {
            get { return _ContractType; }
            set { _ContractType = value; }
        }

        public Customer Customer
        {
            get { return _Customer; }
            set { _Customer = value; }
        }

        public int Customer_id
        {
            get { return _Customer_id; }
            set { _Customer_id = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public decimal Rate
        {
            get { return _Rate; }
            set { _Rate = value; }
        }

        public int Type_id
        {
            get { return _Type_id; }
            set { _Type_id = value; }
        }


        #endregion
    }

    public class CustomerLaborRate
    {
        #region

        private Customer _Customer;
        private int _Customer_id;
        private Labor _Labor;
        private int _Labor_id;
        private System.Nullable<decimal> _Rate;


        #endregion

        #region


        public Customer Customer
        {
            get { return _Customer; }
            set { _Customer = value; }
        }

        public int Customer_id
        {
            get { return _Customer_id; }
            set { _Customer_id = value; }
        }

        public Labor Labor
        {
            get { return _Labor; }
            set { _Labor = value; }
        }

        public int Labor_id
        {
            get { return _Labor_id; }
            set { _Labor_id = value; }
        }

        public System.Nullable<decimal> Rate
        {
            get { return _Rate; }
            set { _Rate = value; }
        }


        #endregion
    }

    public class Employee
    {
        #region

        private string _Email;
        private ICollection<EmployeeRole> _EmployeeRole;
        private string _F_name;
        private int _Id;
        private string _L_name;
        private ICollection<TicketStatusEvent> _TicketStatusEvent;


        #endregion

        #region


        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public ICollection<EmployeeRole> EmployeeRole
        {
            get { return _EmployeeRole; }
            set { _EmployeeRole = value; }
        }

        public string F_name
        {
            get { return _F_name; }
            set { _F_name = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string L_name
        {
            get { return _L_name; }
            set { _L_name = value; }
        }

        public ICollection<TicketStatusEvent> TicketStatusEvent
        {
            get { return _TicketStatusEvent; }
            set { _TicketStatusEvent = value; }
        }


        #endregion
    }

    public class EmployeeRole
    {
        #region

        private Employee _Employee;
        private int _Employee_id;
        private Role _Role;
        private int _Role_id;


        #endregion

        #region


        public Employee Employee
        {
            get { return _Employee; }
            set { _Employee = value; }
        }

        public int Employee_id
        {
            get { return _Employee_id; }
            set { _Employee_id = value; }
        }

        public Role Role
        {
            get { return _Role; }
            set { _Role = value; }
        }

        public int Role_id
        {
            get { return _Role_id; }
            set { _Role_id = value; }
        }


        #endregion
    }

    public class Labor
    {
        #region

        private ICollection<CustomerLaborRate> _CustomerLaborRate;
        private string _Description;
        private int _Id;
        private string _Labor1;
        private LaborRate _LaborRate;
        private ICollection<TicketType> _TicketType;


        #endregion

        #region


        public ICollection<CustomerLaborRate> CustomerLaborRate
        {
            get { return _CustomerLaborRate; }
            set { _CustomerLaborRate = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Labor1
        {
            get { return _Labor1; }
            set { _Labor1 = value; }
        }

        public LaborRate LaborRate
        {
            get { return _LaborRate; }
            set { _LaborRate = value; }
        }

        public ICollection<TicketType> TicketType
        {
            get { return _TicketType; }
            set { _TicketType = value; }
        }


        #endregion
    }

    public class LaborRate
    {
        #region

        private Labor _Labor;
        private int _Labor_id;
        private System.Nullable<decimal> _Rate;


        #endregion

        #region


        public Labor Labor
        {
            get { return _Labor; }
            set { _Labor = value; }
        }

        public int Labor_id
        {
            get { return _Labor_id; }
            set { _Labor_id = value; }
        }

        public System.Nullable<decimal> Rate
        {
            get { return _Rate; }
            set { _Rate = value; }
        }


        #endregion
    }

    public class Role
    {
        #region

        private ICollection<EmployeeRole> _EmployeeRole;
        private int _Id;
        private string _Role1;


        #endregion

        #region


        public ICollection<EmployeeRole> EmployeeRole
        {
            get { return _EmployeeRole; }
            set { _EmployeeRole = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Role1
        {
            get { return _Role1; }
            set { _Role1 = value; }
        }


        #endregion
    }

    public class Ticket
    {
        #region

        private Customer _Customer;
        private int _Customer_id;
        private string _Description;
        private int _Id;
        private ICollection<TicketNote> _TicketNote;
        private ICollection<TicketStatusEvent> _TicketStatusEvent;
        private TicketType _TicketType;


        #endregion

        #region


        public Customer Customer
        {
            get { return _Customer; }
            set { _Customer = value; }
        }

        public int Customer_id
        {
            get { return _Customer_id; }
            set { _Customer_id = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public ICollection<TicketNote> TicketNote
        {
            get { return _TicketNote; }
            set { _TicketNote = value; }
        }

        public ICollection<TicketStatusEvent> TicketStatusEvent
        {
            get { return _TicketStatusEvent; }
            set { _TicketStatusEvent = value; }
        }

        public TicketType TicketType
        {
            get { return _TicketType; }
            set { _TicketType = value; }
        }


        #endregion
    }

    public class TicketNote
    {
        #region

        private int _Id;
        private string _Note;
        private Ticket _Ticket;
        private int _Ticket_id;


        #endregion

        #region


        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Note
        {
            get { return _Note; }
            set { _Note = value; }
        }

        public Ticket Ticket
        {
            get { return _Ticket; }
            set { _Ticket = value; }
        }

        public int Ticket_id
        {
            get { return _Ticket_id; }
            set { _Ticket_id = value; }
        }


        #endregion
    }

    public class TicketStatus
    {
        #region

        private string _Description;
        private int _Id;
        private string _Status;
        private ICollection<TicketStatusEvent> _TicketStatusEvent;


        #endregion

        #region


        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public ICollection<TicketStatusEvent> TicketStatusEvent
        {
            get { return _TicketStatusEvent; }
            set { _TicketStatusEvent = value; }
        }


        #endregion
    }

    public class TicketStatusEvent
    {
        #region

        private Employee _Employee;
        private int _Employee_id;
        private int _Status_id;
        private Ticket _Ticket;
        private int _Ticket_id;
        private TicketStatus _TicketStatus;
        private System.DateTime _Time;


        #endregion

        #region


        public Employee Employee
        {
            get { return _Employee; }
            set { _Employee = value; }
        }

        public int Employee_id
        {
            get { return _Employee_id; }
            set { _Employee_id = value; }
        }

        public int Status_id
        {
            get { return _Status_id; }
            set { _Status_id = value; }
        }

        public Ticket Ticket
        {
            get { return _Ticket; }
            set { _Ticket = value; }
        }

        public int Ticket_id
        {
            get { return _Ticket_id; }
            set { _Ticket_id = value; }
        }

        public TicketStatus TicketStatus
        {
            get { return _TicketStatus; }
            set { _TicketStatus = value; }
        }

        public System.DateTime Time
        {
            get { return _Time; }
            set { _Time = value; }
        }


        #endregion
    }

    public class TicketType
    {
        #region

        private Labor _Labor;
        private int _Labor_id;
        private Ticket _Ticket;
        private int _Ticket_id;


        #endregion

        #region


        public Labor Labor
        {
            get { return _Labor; }
            set { _Labor = value; }
        }

        public int Labor_id
        {
            get { return _Labor_id; }
            set { _Labor_id = value; }
        }

        public Ticket Ticket
        {
            get { return _Ticket; }
            set { _Ticket = value; }
        }

        public int Ticket_id
        {
            get { return _Ticket_id; }
            set { _Ticket_id = value; }
        }


        #endregion
    }


}