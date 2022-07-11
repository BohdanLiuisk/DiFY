using DiFY.Modules.Social.Application.Contracts;
using System;

namespace DiFY.Modules.Social.Application.FriendshipRequests.CreateFriendshipRequest
{
    public class CreateFriendshipRequestCommand : CommandBase<Guid>
    {
        public CreateFriendshipRequestCommand(Guid requesterId, Guid addresseeId, DateTime createdDate)
        {
            RequesterId = requesterId;
            AddresseeId = addresseeId;
            CreateDate = createdDate;
        }

        public Guid RequesterId { get; }

        public Guid AddresseeId { get; }

        public DateTime CreateDate { get; }
    }
}
