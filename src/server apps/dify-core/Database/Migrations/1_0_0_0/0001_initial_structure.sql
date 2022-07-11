GO
CREATE SCHEMA [users]
    AUTHORIZATION [dbo];

GO
PRINT N'Creating [users].[UserRoles]...';

GO
CREATE TABLE [users].[UserRoles] (
    [UserId]   UNIQUEIDENTIFIER NOT NULL,
    [RoleCode] NVARCHAR (50)    NULL
);

GO
PRINT N'Creating [users].[UserRegistrations]...';

GO
CREATE TABLE [users].[UserRegistrations] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Login]         NVARCHAR (100)   NOT NULL,
    [Email]         NVARCHAR (255)   NOT NULL,
    [Password]      NVARCHAR (255)   NOT NULL,
    [FirstName]     NVARCHAR (50)    NOT NULL,
    [LastName]      NVARCHAR (50)    NOT NULL,
    [Name]          NVARCHAR (255)   NOT NULL,
    [StatusCode]    VARCHAR (50)     NOT NULL,
    [RegisterDate]  DATETIME         NOT NULL,
    [ConfirmedDate] DATETIME         NULL,
    CONSTRAINT [PK_users_UserRegistrations_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
PRINT N'Creating [users].[Users]...';

GO
CREATE TABLE [users].[Users] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Login]     NVARCHAR (100)   NOT NULL,
    [Email]     NVARCHAR (255)   NOT NULL,
    [Password]  NVARCHAR (255)   NOT NULL,
    [IsActive]  BIT              NOT NULL,
    [FirstName] NVARCHAR (50)    NOT NULL,
    [LastName]  NVARCHAR (50)    NOT NULL,
    [Name]      NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_users_Users_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
PRINT N'Creating [users].[RolesToPermissions]...';

GO
CREATE TABLE [users].[RolesToPermissions] (
    [RoleCode]       VARCHAR (50) NOT NULL,
    [PermissionCode] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_RolesToPermissions_RoleCode_PermissionCode] PRIMARY KEY CLUSTERED ([RoleCode] ASC, [PermissionCode] ASC)
);

GO
PRINT N'Creating [users].[Permissions]...';

GO
CREATE TABLE [users].[Permissions] (
    [Code]        VARCHAR (50)  NOT NULL,
    [Name]        VARCHAR (100) NOT NULL,
    [Description] VARCHAR (255) NULL,
    CONSTRAINT [PK_users_Permissions_Code] PRIMARY KEY CLUSTERED ([Code] ASC)
);

GO
PRINT N'Creating [users].[v_UserRoles]...';

GO
CREATE VIEW [users].[v_UserRoles]
AS
SELECT
    [UserRole].[UserId],
    [UserRole].[RoleCode]
FROM [users].[UserRoles] AS [UserRole]

GO
PRINT N'Creating [users].[v_Users]...';

GO
CREATE VIEW [users].[v_Users]
AS
SELECT
    [User].[Id],
    [User].[IsActive],
    [User].[Login],
    [User].[Password],
    [User].[Email],
    [User].[Name]
FROM [users].[Users] AS [User]
GO
PRINT N'Creating [users].[v_UserRegistrations]...';

GO
CREATE VIEW [users].[v_UserRegistrations]
AS
SELECT
    [UserRegistration].[Id],
    [UserRegistration].[Login],
    [UserRegistration].[Email],
    [UserRegistration].[FirstName],
    [UserRegistration].[LastName],
    [UserRegistration].[Name],
    [UserRegistration].[StatusCode]
FROM [users].[UserRegistrations] AS [UserRegistration]

GO
PRINT N'Creating [users].[v_UserPermissions]...';

GO
CREATE VIEW [users].[v_UserPermissions]
AS
SELECT 
	DISTINCT
	[UserRole].UserId,
	[RolesToPermission].PermissionCode
FROM [users].UserRoles AS [UserRole]
	INNER JOIN [users].RolesToPermissions AS [RolesToPermission]
		ON [UserRole].RoleCode = [RolesToPermission].RoleCode

GO
PRINT N'Creating [administration]...';

GO
CREATE SCHEMA [administration]
    AUTHORIZATION [dbo];

GO
PRINT N'Creating [administration].[Members]...';

GO
CREATE TABLE [administration].[Members] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Login]     NVARCHAR (100)   NOT NULL,
    [Email]     NVARCHAR (255)   NOT NULL,
    [FirstName] NVARCHAR (50)    NOT NULL,
    [LastName]  NVARCHAR (50)    NOT NULL,
    [Name]      NVARCHAR (255)   NOT NULL,
    CONSTRAINT [PK_administration_Members_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
PRINT N'Update complete.';

GO