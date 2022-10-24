GO
ALTER TABLE [social].[Calls]
    ADD [Active] BIT;

GO
ALTER TABLE [social].[Calls]
    ADD [DropperId] UNIQUEIDENTIFIER 
    CONSTRAINT [FK_social_Calls_DropperId] FOREIGN KEY([DropperId]) REFERENCES [social].[Members]([Id]);

GO
ALTER TABLE [social].[CallParticipants]
    ADD [Active] BIT;

GO
ALTER TABLE [social].[Calls]
    DROP COLUMN [Duration];

GO
ALTER TABLE [social].[Calls]
    ADD [Duration] FLOAT NULL;
