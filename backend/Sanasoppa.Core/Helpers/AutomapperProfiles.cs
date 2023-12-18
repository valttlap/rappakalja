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
        CreateMap<Player, PlayerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.GameSessionId, opt => opt.MapFrom(src => src.GameSessionId.ToString()))
            .ForMember(dest => dest.ConnectionId, opt => opt.MapFrom(src => src.ConnectionId))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Scores.Sum(s => s.Points)));
        CreateMap<PlayerDto, Player>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.GameSessionId, opt => opt.MapFrom(src => Guid.Parse(src.GameSessionId)))
            .ForMember(dest => dest.ConnectionId, opt => opt.MapFrom(src => src.ConnectionId));

    }
}
