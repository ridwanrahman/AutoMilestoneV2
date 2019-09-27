select * from AspNetUsers;

delete from AspNetUsers where email='ridwanrahman07@gmail.com';

alter table vehicle add image_path varchar(255);

select * from Vehicle;

select * from userrolesbridging;

insert into [dbo].[userrolesbridging] values ('bd44b938-8f13-45c4-96ee-08dd988b5d9c','2');

insert into [dbo].[Vehicle] ([Name],[Model],[userId],[image_path]) Values ('mazda', '2003', 'bd44b938-8f13-45c4-96ee-08dd988b5d9c', 'file.jpg');



create table [dbo].[userrolesbridging](
	[Id] int IDENTITY(1,1) NOT NULL,
	[userid] NVARCHAR (128) NOT NULL,
	[roleid] NVARCHAR (128) NOT NULL,
	
	PRIMARY KEY(Id),
	FOREIGN KEY(userid) REFERENCES AspNetUsers(Id),
	FOREIGN KEY(roleid) REFERENCES AspNetRoles(Id),
);

CREATE TABLE [dbo].[userrolesbridging] (
	[Id] int IDENTITY(1,1) NOT NULL,
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.userrolesbridging] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.userrolesbridging_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.userrolesbridging_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);