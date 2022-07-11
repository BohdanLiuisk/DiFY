using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.UserAccess.Domain.UserRegistrations
{
    public class UserRegistrationStatus : ValueObject
    {
        public static UserRegistrationStatus WaitingForConfirmation => new(nameof(WaitingForConfirmation));

        public static UserRegistrationStatus Confirmed => new(nameof(Confirmed));

        public static UserRegistrationStatus Expired => new(nameof(Expired));
        
        public string Value { get; }
        
        private UserRegistrationStatus(string value)
        {
            Value = value;
        }
    }
}