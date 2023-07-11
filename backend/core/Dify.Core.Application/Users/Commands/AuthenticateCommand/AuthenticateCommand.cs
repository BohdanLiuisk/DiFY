using MediatR;

namespace Dify.Core.Application.Users.Commands.AuthenticateCommand;

public record AuthenticateCommand(string Login, string Password) : IRequest<AuthenticationResult>;
