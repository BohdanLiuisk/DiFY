using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.UserAccess.Domain.UserRegistrations.Events
{
    public class UserRegistrationConfirmedDomainEvent : DomainEventBase
    {
        public UserRegistrationConfirmedDomainEvent(UserRegistrationId userRegistrationId)
        {
            UserRegistrationId = userRegistrationId;
        }
        
        public UserRegistrationId UserRegistrationId { get; }
    }
}