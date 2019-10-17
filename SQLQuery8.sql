select * from AspNetUsers;
select * from userrolesbridging;
select * from AspNetRoles;

delete from AspNetUsers where id='6cb4cd6d-2d8c-4b33-bbbd-7b0f58b8a59d';

select * from CustomerBooking;

select * from CustomerBooking where from_date between'2019-10-28' and '2019-10-30' and 
to_date between '2019-10-28' and '2019-10-30' and vehicle_id=43;

select * from CustomerBooking where
(from_date between '2019-10-28' and '2019-10-31') or
(to_date between '2019-10-28' and '2019-10-31') or
(from_date <= '2019-10-28' and to_date >= '2019-10-31') and vehicle_id=43;

select * from Vehicle;

select * from CustomerBookingLocation;




