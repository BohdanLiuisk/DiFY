GO
ALTER TABLE [social].[CallParticipants]
    ADD [StreamId] VARCHAR(50) NULL;

GO
ALTER TABLE [social].[CallParticipants]
    ADD [PeerId] VARCHAR(50) NULL;

GO
ALTER TABLE [social].[CallParticipants]
    ADD [ConnectionId] VARCHAR(50) NULL;
