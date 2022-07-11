using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.UserAccess.Domain.UserRegistrations
{
    public class UserRegistrationId : TypedIdValueBase
    {
        public UserRegistrationId(Guid value) : base(value)
        {
            
        }
    }
}