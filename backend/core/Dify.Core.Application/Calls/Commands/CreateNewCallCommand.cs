using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Domain.Entities;

namespace Dify.Core.Application.Calls.Commands;

public record CreateNewCallCommand(
    string Name
): IRequest<CommandResponse<NewCallResponse>>;

public record NewCallResponse(Guid CallId);

public class CreateNewCallCommandHandler : IRequestHandler<CreateNewCallCommand, CommandResponse<NewCallResponse>>
{
    private readonly IDifyContext _difyContext;

    public CreateNewCallCommandHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }

    public async Task<CommandResponse<NewCallResponse>> Handle(CreateNewCallCommand command, CancellationToken cancellationToken)
    {
        var call = Call.CreateNew(command.Name);
        await _difyContext.Calls.AddAsync(call, cancellationToken);
        await _difyContext.SaveChangesAsync(cancellationToken);
        return new CommandResponse<NewCallResponse>(new NewCallResponse(call.Id));
    }
}
