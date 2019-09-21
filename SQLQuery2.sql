select * from AspNetUsers;

select * from AspNetRoles;

insert into [dbo].[AspNetUserRoles]([UserId], [RoleId]) values ('9b39484e-a16d-4917-9d0b-88864bb91c38',1);

select * from AspNetUserRoles;

select roles.Name from AspNetUserRoles ur join AspNetUsers r on ur.UserId=r.Id
			join AspNetRoles roles on ur.RoleId=roles.Id where r.Email='user@monash.edu'; 