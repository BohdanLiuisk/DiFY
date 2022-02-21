using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.FriendshipRequests
{
    public class FriendshipRequestStatus : ValueObject
    {
        public static FriendshipRequestStatus Waiting => new(nameof(Waiting));

        public static FriendshipRequestStatus Confirmed => new(nameof(Confirmed));

        public static FriendshipRequestStatus Rejected => new(nameof(Rejected));

        public string Value { get; }

        private FriendshipRequestStatus(string value)
        {
            Value = value;
        }
    }
}
