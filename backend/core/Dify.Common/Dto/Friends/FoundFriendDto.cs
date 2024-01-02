namespace Dify.Common.Dto.Friends;

public record FoundFriendDto (
    int Id,
    string Name,
    string AvatarUrl,
    int MutualFriendsCount
);
