create table [dbo].[CustomerBookingLocation](
	[Id] int IDENTITY(1,1) NOT NULL,
	[customer_booking_id] int NOT NULL,
	[latitude] numeric (10,8) NOT NULL,
	[longitude] numeric (11, 8) NOT NULL,
	CONSTRAINT Chk_Lat CHECK (latitude >= -90 AND latitude <=90),
	CONSTRAINT Chk_Lon CHECK (longitude >= -180 AND longitude <= 180),

	primary key(Id),
	FOREIGN KEY ([customer_booking_id]) REFERENCES CustomerBooking([customer_booking_id]),
);


select * from CustomerBooking;