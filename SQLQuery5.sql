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

alter table [dbo].[CustomerBooking] add isAccepted nvarchar(max) NOT NULL;

alter table vehicle add vehicle_description nvarchar(max) NULL;

select * from vehicle;

delete from vehicle where id =6;