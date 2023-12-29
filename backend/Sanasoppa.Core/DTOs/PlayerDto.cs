using System.ComponentModel.DataAnnotations;

namespace Sanasoppa.Core.DTOs;

public class PlayerDto
{
    [Required]
    public string Id { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string GameSessionId { get; set; } = null!;

    [Required]
    public string ConnectionId { get; set; } = null!;

    [Required]
    public string PlayerId { get; set; } = null!;
}
