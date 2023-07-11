﻿using AutoMapper;
using Dify.Core.Application.Users.Commands.CreateNewUser;
using Dify.Core.Domain.Entities;

namespace Dify.Core.Application.MappingProfiles;

public sealed class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<CreateNewUserCommand, User>()
            .ForMember(dest => dest.Name, opt => 
                opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
