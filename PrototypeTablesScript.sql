-- MSSQL Table structure for Compsys Ticketing app.

CREATE TABLE Employee (
	id int IDENTITY(1,1) PRIMARY KEY,
	email varchar(64) NOT NULL,
	f_name varchar(64) NOT NULL,
	l_name varchar(64) NOT NULL
);
CREATE TABLE Role (
	id int IDENTITY(1,1) PRIMARY KEY,
	role varchar(128) NOT NULL
);
CREATE TABLE EmployeeRole (
	employee_id int NOT NULL,
	role_id int NOT NULL,
	FOREIGN KEY(employee_id) REFERENCES Employee (id),
	FOREIGN KEY(role_id) REFERENCES Role (id),
	CONSTRAINT pk_employeerole PRIMARY KEY (employee_id, role_id)
);
CREATE TABLE Customer (
	id int IDENTITY(1,1) PRIMARY KEY,
	name varchar(128) NOT NULL,
	phone varchar(32) NOT NULL,
	fax varchar(32)
);
CREATE TABLE Ticket (
	id int IDENTITY(1,1) PRIMARY KEY,
	description text NOT NULL,
	customer_id int NOT NULL,
	FOREIGN KEY(customer_id) REFERENCES Customer (id)
);
CREATE TABLE TicketStatus (
	id int IDENTITY(1,1) PRIMARY KEY,
	status varchar(128),
	description text
);
CREATE TABLE TicketStatusEvent (
	ticket_id int NOT NULL,
	time timestamp,
	employee_id int NOT NULL,
	status_id int NOT NULL,
	FOREIGN KEY(ticket_id) REFERENCES Ticket (id),
	FOREIGN KEY(employee_id) REFERENCES Employee (id),
	FOREIGN KEY(status_id) REFERENCES TicketStatus (id),
	CONSTRAINT pk_ticketsstatusevent PRIMARY KEY (ticket_id, time)
);
CREATE TABLE ContractType (
	id int IDENTITY(1,1) PRIMARY KEY,
	type varchar(128) NOT NULL,
	description text NOT NULL
);
CREATE TABLE CustomerContract (
	id int IDENTITY(1,1) PRIMARY KEY,
	rate smallmoney NOT NULL,
	customer_id int NOT NULL,
	type_id int NOT NULL,
	FOREIGN KEY(customer_id) REFERENCES Customer (id),
	FOREIGN KEY(type_id) REFERENCES ContractType (id)
);
CREATE TABLE TicketNote (
	id int IDENTITY(1,1) PRIMARY KEY,
	ticket_id int NOT NULL,
	note text NOT NULL,
	FOREIGN KEY(ticket_id) REFERENCES Ticket (id)
);
CREATE TABLE TicketType (
	ticket_id int NOT NULL PRIMARY KEY,
	labor_id int NOT NULL,
	FOREIGN KEY(ticket_id) REFERENCES Ticket (id)
	FOREIGN KEY(labor_id) REFERENCES Labor (id)
);
CREATE TABLE Labor (
	id int IDENTITY(1,1) PRIMARY KEY,
	labor varchar(128) NOT NULL,
	description text NOT NULL
);
CREATE TABLE LaborRate (
	labor_id int PRIMARY KEY,
	rate smallmoney,
	FOREIGN KEY(labor_id) REFERENCES Labor (id)
);
CREATE TABLE CustomerLaborRate (
	customer_id int,
	labor_id int,
	rate smallmoney,
	FOREIGN KEY(customer_id) REFERENCES Customer (id),
	FOREIGN KEY(labor_id) REFERENCES Labor (id),
	CONSTRAINT pk_customerlaborrate PRIMARY KEY (customer_id, labor_id)
);
