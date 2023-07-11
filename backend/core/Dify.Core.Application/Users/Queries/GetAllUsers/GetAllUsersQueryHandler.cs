using Dify.Core.Application.Common.Interfaces;
using Dify.Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dify.Core.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    private readonly IDifyContext _difyContext;

    public GetAllUsersQueryHandler(IDifyContext difyContext)
    {
        _difyContext = difyContext;
    }

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        return await _difyContext.Users.ToListAsync(cancellationToken);
    }
}
