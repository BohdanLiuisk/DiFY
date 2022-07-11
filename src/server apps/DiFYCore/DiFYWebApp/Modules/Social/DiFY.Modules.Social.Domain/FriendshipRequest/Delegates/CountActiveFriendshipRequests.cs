using System;

namespace DiFY.Modules.Social.Domain.FriendshipRequests.Delegates;

public delegate int CountActiveFriendshipRequests(Guid firstParticipant, Guid secondParticipant);
