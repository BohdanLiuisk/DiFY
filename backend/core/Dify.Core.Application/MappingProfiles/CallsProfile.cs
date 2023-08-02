using Dify.Common.Dto;
using Dify.Core.Application.Calls.Commands;
using Dify.Core.Domain.Entities;

namespace Dify.Core.Application.MappingProfiles;

public sealed class CallsProfile : Profile
{
    public CallsProfile()
    {
        CreateMap<Call, CallDto>()
            .ForMember(dest => dest.StartDate, 
                opt => opt.MapFrom(src => src.CreatedOn))
            .ForMember(dest => dest.InitiatorId, 
                opt => opt.MapFrom(src => src.CreatedById))
            .ForMember(dest => dest.TotalParticipants, 
                opt => opt.MapFrom(src => src.Participants.Count))
            .ForMember(dest => dest.ActiveParticipants, 
                opt => opt.MapFrom(src => src.Participants.Count(p => p.Active)));

        CreateMap<CallParticipant, CallParticipantDto>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Participant.Name));
        
        CreateMap<JoinCallCommand, CallParticipant>();
    }
}
