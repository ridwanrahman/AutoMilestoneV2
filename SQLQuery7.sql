select * from CustomerBooking;

delete from CustomerBooking where customer_booking_id=2;

select * from CustomerBookingLocation;

alter table CustomerBookingLocation drop column longitude;

alter table CustomerBookingLocation add latitude nvarchar(MAX) NULL;

select * from CustomerBooking;