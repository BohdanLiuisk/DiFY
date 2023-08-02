namespace Dify.Core.Application.Common;

public interface ICurrentUser
{
    int UserId { get; }
    
    bool IsAvailable { get; }
}
