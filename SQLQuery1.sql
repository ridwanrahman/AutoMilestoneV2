--sql command to create table with foreign key
create table [dbo].[Vehicle](
	[Id] int IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(max) NOT NULL,
	[Model] nvarchar(max) NOT NULL,
	[userId] nvarchar(128) NOT NULL,
	primary key(Id),
	FOREIGN KEY (userId) REFERENCES AspNetUsers(Id)
);
Go

alter table aspnetroles add RoleDescription nvarchar(255) NULL;

select * from AspNetRoles;
-- insert statments
insert into AspNetRoles([Id], [Name], [RoleDescription]) 
values (2, 'Staff', 'Staff');

insert into AspNetRoles([Id], [Name], [RoleDescription]) 
values (3, 'Customer', 'Customer');


