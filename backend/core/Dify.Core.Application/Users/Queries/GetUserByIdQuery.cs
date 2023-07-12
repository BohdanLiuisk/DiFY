using AutoMapper;
using Dify.Common.Dto;
using Dify.Common.Models;
using Dify.Core.Application.Common;
using Dify.Core.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dify.Core.Application.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<QueryResponse<UserDto>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, QueryResponse<UserDto>>
{
    private readonly IDifyContext _difyContext;
    
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IDifyContext difyContext, IMapper mapper)
    {
        _difyContext = difyContext;
        _mapper = mapper;
    }
    
    public async Task<QueryResponse<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _difyContext.Users.FirstOrDefaultAsync(u => u.Id == query.Id, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User with id {query.Id} was not found.");
        }
        var response = new QueryResponse<UserDto>();
        response.SetBody(_mapper.Map<UserDto>(user));
        return response;
    }
}
