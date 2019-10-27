------------To run the ASP.NET application-----------------
1. Load up the project
2. Make a database in app_data
3. create the AspNetUsers and similar tables according to the asp.net social authentication tutorial
4. Run the below sql commands to generate the required tables because I followed the database first methodology.

--sql command to create table with foreign key
create table [dbo].[Vehicle](
	[Id] int IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(max) NOT NULL,
	[Model] nvarchar(max) NOT NULL,
	[userId] nvarchar(128) NOT NULL,
	primary key(Id),
	FOREIGN KEY (userId) REFERENCES AspNetUsers(Id)
);
create table [dbo].[Booking] (
	[booking_id] int IDENTITY(1,1) NOT NULL,
	[date] nvarchar(max) NOT NULL,
	[time] nvarchar(max) NOT NULL,
	[pickup_location] nvarchar(max) NOT NULL,
	[dropoff_location] nvarchar(max) NOT NULL,
	[isAccepted] nvarchar(max) NOT NULL,
	primary key(booking_id),
);


create table [dbo].[CustomerBooking](
	[customer_booking_id] int IDENTITY(1,1) NOT NULL,
	[userId] nvarchar(128) NOT NULL,
	[booking_id] int NOT NULL,
	[vehicle_id] int NOT NULL,
	primary key(customer_booking_id),
	FOREIGN KEY (userId) REFERENCES AspNetUsers(Id),
	FOREIGN KEY ([booking_id]) REFERENCES Booking(booking_id),
	FOREIGN KEY (vehicle_id) REFERENCES Vehicle(Id)
);
Go

create table [dbo].[userrolesbridging](
	[Id] int IDENTITY(1,1) NOT NULL,
	[userid] NVARCHAR (128) NOT NULL,
	[roleid] NVARCHAR (128) NOT NULL,
	
	PRIMARY KEY(Id),
	FOREIGN KEY(userid) REFERENCES AspNetUsers(Id),
	FOREIGN KEY(roleid) REFERENCES AspNetRoles(Id),
);