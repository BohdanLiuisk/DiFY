GO
CREATE SCHEMA [social]
    AUTHORIZATION [dbo];

GO
CREATE TABLE [social].[Friendship]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [RequesterId]  UNIQUEIDENTIFIER NOT NULL,
    [AddresseeId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn] DATETIME NOT NULL,
    CONSTRAINT [PK_social_Friendship_Id] PRIMARY KEY ([Id] ASC),
    CONSTRAINT [FK_social_Friendship_Requester] FOREIGN KEY ([RequesterId]) REFERENCES administration.Members ([Id]),
    CONSTRAINT [FK_social_Friendship_Addressee] FOREIGN KEY ([AddresseeId]) REFERENCES administration.Members ([Id])
)

GO
CREATE TABLE [social].[FriendshipRequest]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [RequesterId]  UNIQUEIDENTIFIER NOT NULL,
    [AddresseeId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn] DATETIME NOT NULL,
    [ConfirmedOn] DATETIME NULL,
    [RejectedOn] DATETIME NULL,
    CONSTRAINT [PK_social_FriendshipRequest_Id] PRIMARY KEY ([Id] ASC),
    CONSTRAINT [FK_social_FriendshipRequest_Requester] FOREIGN KEY ([RequesterId]) REFERENCES administration.Members ([Id]),
    CONSTRAINT [FK_social_FriendshipRequest_Addressee] FOREIGN KEY ([AddresseeId]) REFERENCES administration.Members ([Id])
)
