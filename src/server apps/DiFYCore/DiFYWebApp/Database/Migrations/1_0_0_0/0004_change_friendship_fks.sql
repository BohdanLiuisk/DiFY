GO
ALTER TABLE [social].[Friendship]
DROP CONSTRAINT FK_social_Friendship_Requester;

GO
ALTER TABLE [social].[Friendship]
DROP CONSTRAINT FK_social_Friendship_Addressee;

GO
ALTER TABLE [social].[Friendship]
ADD CONSTRAINT FK_social_Friendship_Requester FOREIGN KEY ([RequesterId])
    REFERENCES [users].[Users]([Id])

GO
ALTER TABLE [social].[Friendship]
ADD CONSTRAINT FK_social_Friendship_Addressee FOREIGN KEY ([AddresseeId])
    REFERENCES [users].[Users]([Id])