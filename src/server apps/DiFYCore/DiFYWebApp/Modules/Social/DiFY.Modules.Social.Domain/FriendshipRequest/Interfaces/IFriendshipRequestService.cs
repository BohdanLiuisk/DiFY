using System;

namespace DiFY.Modules.Social.Domain.FriendshipRequests.Interfaces
{
    public interface IFriendshipRequestService
    {
        int GetFriendshipRequestsCount(Guid firstParticipant, Guid secondParticipant);
    }
}
