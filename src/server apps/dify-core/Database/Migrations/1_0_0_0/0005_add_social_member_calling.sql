GO
CREATE TABLE [social].[Members] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Login] NVARCHAR (100) NOT NULL,
    [Email] NVARCHAR (255) NOT NULL,
    [FirstName] NVARCHAR (50) NOT NULL,
    [LastName] NVARCHAR (50) NOT NULL,
    [Name] NVARCHAR (255) NOT NULL,
    [CreatedOn] DATETIME NOT NULL,
    CONSTRAINT [PK_social_Members_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [social].[Calls] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [InitiatorId] UNIQUEIDENTIFIER NOT NULL,
    [StartDate] DATETIME NOT NULL,
    [EndDate] DATETIME NULL,
    [Duration] FLOAT(2) NULL
    CONSTRAINT [PK_social_Calls_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_social_Calls_InitiatorId] FOREIGN KEY ([InitiatorId]) REFERENCES [social].[Members]([Id])
);

GO
CREATE TABLE [social].[CallParticipants] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [CallId] UNIQUEIDENTIFIER NOT NULL,
    [ParticipantId] UNIQUEIDENTIFIER NOT NULL,
    [JoinOn] DATETIME NOT NULL,
    CONSTRAINT [PK_social_CallParticipants_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_social_CallParticipants_CallId] FOREIGN KEY ([CallId]) REFERENCES [social].[Calls] ([Id]),
    CONSTRAINT [FK_social_CallParticipants_ParticipantId] FOREIGN KEY ([ParticipantId]) REFERENCES [social].[Members]([Id])
);

GO
ALTER TABLE [social].[Friendship]
DROP CONSTRAINT [FK_social_Friendship_Requester];

GO
ALTER TABLE [social].[Friendship]
ADD CONSTRAINT [FK_social_Friendship_Requester] FOREIGN KEY ([RequesterId])
    REFERENCES [social].[Members]([Id]);

GO
ALTER TABLE [social].[Friendship]
DROP CONSTRAINT [FK_social_Friendship_Addressee];

GO
ALTER TABLE [social].[Friendship]
ADD CONSTRAINT [FK_social_Friendship_Addressee] FOREIGN KEY ([AddresseeId])
    REFERENCES [social].[Members]([Id]);

GO
ALTER TABLE [social].[FriendshipRequest]
DROP CONSTRAINT [FK_social_FriendshipRequest_Requester];

GO
ALTER TABLE [social].[FriendshipRequest]
ADD CONSTRAINT [FK_social_FriendshipRequest_Requester] FOREIGN KEY ([RequesterId])
    REFERENCES [social].[Members]([Id]);

GO
ALTER TABLE [social].[FriendshipRequest]
DROP CONSTRAINT [FK_social_FriendshipRequest_Addressee];

GO
ALTER TABLE [social].[FriendshipRequest]
ADD CONSTRAINT [FK_social_FriendshipRequest_Addressee] FOREIGN KEY ([AddresseeId])
    REFERENCES [social].[Members]([Id]);
