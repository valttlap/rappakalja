using AutoMapper;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Model.Entities;

namespace Sanasoppa.Core.Helpers;

public class AutomapperProfiles : Profile
{
    public AutomapperProfiles()
    {
        CreateMap<GameSession, GameSessionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players))
            .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src => src.Rounds))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));
    }
}
