GO
ALTER TABLE [social].[Calls]
    ADD [Active] BIT DEFAULT 0;

GO
ALTER TABLE [social].[Calls]
    ADD [DropperId] UNIQUEIDENTIFIER 
    CONSTRAINT [FK_social_Calls_DropperId] FOREIGN KEY([DropperId]) REFERENCES [social].[Members]([Id]);

GO
ALTER TABLE [social].[CallParticipants]
    ADD [Active] BIT DEFAULT 0;

GO
ALTER TABLE [social].[Calls]
    DROP COLUMN [Duration];

GO
ALTER TABLE [social].[Calls]
    ADD [Duration] FLOAT NOT NULL;

GO
ALTER TABLE  [social].Calls
    ADD CONSTRAINT df_social_Calls_Duration
    DEFAULT 0 FOR  [Duration];

GO
ALTER TABLE [social].[Calls]
    ADD [Name] VARCHAR(500) NULL;

GO
CREATE VIEW [social].[v_Calls] 
AS
SELECT 
	[Call].[Id], 
	[Call].[Name], 
	[Call].[Active], 
	[Call].[StartDate], 
	[Call].[EndDate],
	(
		SELECT COUNT([CallParticipants].[Id]) 
		FROM [social].[CallParticipants] AS [CallParticipants]
		WHERE [CallParticipants].[CallId] = [Call].[Id] AND [CallParticipants].[Active] = 1
	) AS [ActiveParticipants],
	(
		SELECT COUNT([CallParticipants].[Id]) 
		FROM [social].[CallParticipants] AS [CallParticipants]
		WHERE [CallParticipants].[CallId] = [Call].[Id]
	) AS [TotalParticipants]
FROM [social].[Calls] AS [Call];
