using Dify.Core.Application.Common;

namespace Dify.Core.Application.Users.Commands;

public record UserDisconnectedCommand(
    string ConnectionId
) : IRequest;

public class UserDisconnectedCommandHandler : IRequestHandler<UserDisconnectedCommand>
{
    private readonly IDifyContext _difyContext;
    
    public UserDisconnectedCommandHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }
    
    public async Task Handle(UserDisconnectedCommand command, CancellationToken cancellationToken)
    {
        var user = await _difyContext.Users.FirstOrDefaultAsync(
            u => u.ConnectionId == command.ConnectionId, cancellationToken);
        if (user is not null)
        {
            user.ConnectionId = null;
            user.Online = false;
            await _difyContext.SaveChangesAsync(cancellationToken);
        }
    }
}
