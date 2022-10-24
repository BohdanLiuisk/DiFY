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

ALTER TABLE  [social].Calls
    ADD CONSTRAINT df_social_Calls_Duration
    DEFAULT 0 FOR  [Duration];