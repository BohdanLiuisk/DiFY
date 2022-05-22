using DiFY.Modules.Social.Application.FriendshipRequest.ConfirmFriendshipRequest;
using System.Reflection;

namespace DiFY.Modules.Social.Infrastructure.Configuration
{
    internal class Assemblies
    {
        public static readonly Assembly Application = typeof(ConfirmFriendshipRequestCommand).Assembly;
    }
}
