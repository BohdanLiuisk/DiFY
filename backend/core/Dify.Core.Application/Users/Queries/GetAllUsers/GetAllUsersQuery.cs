using Dify.Core.Domain.Entities;
using MediatR;

namespace Dify.Core.Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<IEnumerable<User>>;