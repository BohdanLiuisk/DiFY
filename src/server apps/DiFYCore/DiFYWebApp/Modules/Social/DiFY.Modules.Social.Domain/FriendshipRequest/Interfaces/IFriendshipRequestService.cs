namespace DiFY.Modules.Social.Domain.FriendshipRequest.Interfaces
{
    public interface IFriendshipRequestService
    {
        bool FriendshipRequestExists(RequesterId requesterId, AddresseeId addresseeId);

        bool AddresseeToRequesterExists(AddresseeId addresseeId, RequesterId requesterId);
    }
}
