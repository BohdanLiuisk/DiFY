GO
ALTER TABLE [social].[FriendshipRequest]
DROP CONSTRAINT FK_social_FriendshipRequest_Requester;

GO
ALTER TABLE [social].[FriendshipRequest]
DROP CONSTRAINT FK_social_FriendshipRequest_Addressee;

GO
ALTER TABLE [social].[FriendshipRequest]
ADD CONSTRAINT FK_social_FriendshipRequest_Requester FOREIGN KEY ([RequesterId])
    REFERENCES [users].[Users]([Id])

GO
ALTER TABLE [social].[FriendshipRequest]
ADD CONSTRAINT FK_social_FriendshipRequest_Addressee FOREIGN KEY ([AddresseeId])
    REFERENCES [users].[Users]([Id])