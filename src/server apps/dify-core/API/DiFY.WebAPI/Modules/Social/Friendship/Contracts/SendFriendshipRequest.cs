using System;

namespace DiFY.WebAPI.Modules.Social.Friendship.Contracts;

public class SendFriendshipRequest
{
    public Guid RequesterId { get; set; }
    
    public  Guid AddresseeId { get; set; }
}