using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Members.GetUserProfile;

public class GetUserProfileQuery : QueryBase<GetUserProfileDto>
{
    public GetUserProfileQuery(Guid userId)
    {
        UserId = userId;
    }
    
    public Guid UserId { get; set; }
}
