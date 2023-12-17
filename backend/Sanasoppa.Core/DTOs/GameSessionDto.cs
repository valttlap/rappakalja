using System.ComponentModel.DataAnnotations;

namespace Sanasoppa.Core.DTOs;

public class GameSessionDto
{
    [Required]
    public string Id { get; set; } = null!;
    public string? JoinCode { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? OwnerId { get; set; }
    public ICollection<PlayerDto> Players { get; set; } = new List<PlayerDto>();
    public ICollection<RoundDto> Rounds { get; set; } = new List<RoundDto>();
}
