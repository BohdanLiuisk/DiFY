using Dify.Common;
using Dify.Common.Dto;
using MediatR;

namespace Dify.Core.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(int Id) : IRequest<QueryResponse<UserDto>>;
