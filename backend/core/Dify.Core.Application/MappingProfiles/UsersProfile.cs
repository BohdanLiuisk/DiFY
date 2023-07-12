using AutoMapper;
using Dify.Common.Dto;
using Dify.Core.Application.Users.Commands;
using Dify.Core.Domain.Entities;

namespace Dify.Core.Application.MappingProfiles;

public sealed class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<CreateNewUserCommand, User>()
            .ForMember(dest => dest.Name, opt => 
                opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        CreateMap<User, UserDto>();
    }
}
