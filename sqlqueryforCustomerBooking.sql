select * from AspNetUsers;

select * from Vehicle;
select * from Booking;

alter table Booking drop column date;

alter table Booking add to_date datetime NULL;

alter table CustomerBooking add to_date datetime NULL;
alter table CustomerBooking add from_date datetime NULL;
alter table CustomerBooking add dropoff_location NVARCHAR (MAX) NULL;
alter table CustomerBooking drop column booking_id;

select * from CustomerBooking;
select * from Vehicle;

SET IDENTITY_INSERT [dbo].[CustomerBooking] ON
insert into CustomerBooking(customer_booking_id,userId, vehicle_id,
isAccepted,to_date,from_date,pickup_location,dropoff_location) 
values (1,'6cb4cd6d-2d8c-4b33-bbbd-7b0f58b8a59d',
2, 'true', convert(datetime,'19-06-12 10:34:09 PM',5), 
convert(datetime,'19-06-14 10:34:09 PM',5), 'locationA', 'locationB');
SET IDENTITY_INSERT [dbo].[CustomerBooking] OFF

select * from CustomerBooking;