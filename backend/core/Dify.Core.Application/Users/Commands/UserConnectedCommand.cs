using Dify.Core.Application.Common;

namespace Dify.Core.Application.Users.Commands;

public record UserConnectedCommand(
    string ConnectionId,
    int UserId
) : IRequest;

public class UserConnectedCommandHandler : IRequestHandler<UserConnectedCommand>
{
    private readonly IDifyContext _difyContext;
    
    public UserConnectedCommandHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }
    
    public async Task Handle(UserConnectedCommand request, CancellationToken cancellationToken)
    {
        var user = await _difyContext.Users.FindAsync(request.UserId);
        user.ConnectionId = request.ConnectionId;
        user.Online = true;
        await _difyContext.SaveChangesAsync(cancellationToken);
    }
}
