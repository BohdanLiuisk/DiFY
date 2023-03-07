IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'AvatarUrl' AND Object_ID = Object_ID(N'[social].[Members]'))
BEGIN
    ALTER TABLE [social].[Members]
        ADD [AvatarUrl] VARCHAR(56) NULL;
END

IF (OBJECT_ID('[social].[v_Calls]') IS NOT NULL)
	BEGIN
		DROP VIEW [social].[v_Calls]
	END
GO
CREATE VIEW [social].[v_Calls] 
AS
SELECT
	[Call].[Id],
	[Call].[Name],
	[Call].[Active],
	[Call].[StartDate],
	[Call].[EndDate],
	COUNT(CASE WHEN [CallParticipants].[Active] = 1 THEN [CallParticipants].[Id] END) AS [ActiveParticipants],
	COUNT([CallParticipants].[Id]) AS [TotalParticipants]
FROM [social].[Calls] AS [Call]
LEFT JOIN [social].[CallParticipants] AS [CallParticipants] 
ON [Call].[Id] = [CallParticipants].[CallId]
GROUP BY [Call].[Id], [Call].[Name], [Call].[Active], [Call].[StartDate], [Call].[EndDate];

GO

IF (OBJECT_ID('[social].[v_Profile]') IS NOT NULL)
	BEGIN
		DROP VIEW [social].[v_Profile]
	END
GO
CREATE VIEW [social].[v_Profile] 
AS
SELECT
    [profile].[Id] AS [Id], 
    [profile].[Login] AS [Login], 
    [profile].[Email] AS [Email], 
    [profile].[FirstName] AS [FirstName], 
    [profile].[LastName] AS [LastName],
    [profile].[CreatedOn] AS [CreatedOn], 
    [profile].[AvatarUrl] AS [AvatarUrl]
FROM [social].[Members] AS [profile];

GO

DELETE FROM [users].[RolesToPermissions];
DELETE FROM [users].[Permissions];

INSERT INTO [users].[Permissions] ([Code], [Name]) VALUES
	('CanGetUserProfile', 'CanGetUserProfile'),
	('CanSendFriendshipRequest', 'CanSendFriendshipRequest'),
	('CanGetAllCalls', 'CanGetAllCalls'),
	('CanCreateCall', 'CanCreateCall');

INSERT INTO [users].[RolesToPermissions] ([RoleCode], [PermissionCode]) VALUES
	('Member', 'CanGetUserProfile'),
	('Member', 'CanSendFriendshipRequest'),
	('Member', 'CanGetAllCalls'),
	('Member', 'CanCreateCall');
