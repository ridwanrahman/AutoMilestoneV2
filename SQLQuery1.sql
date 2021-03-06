﻿--sql command to create table with foreign key
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

alter table aspnetroles add RoleDescription nvarchar(255) NULL;

select * from AspNetRoles;
-- insert statments
insert into AspNetRoles([Id], [Name], [RoleDescription]) 
values (2, 'Staff', 'Staff');

insert into AspNetRoles([Id], [Name], [RoleDescription]) 
values (3, 'Customer', 'Customer');


