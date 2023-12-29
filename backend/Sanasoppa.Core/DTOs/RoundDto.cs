namespace Sanasoppa.Core.DTOs;

public class RoundDto
{
    public string Id { get; set; } = null!;

    public string GameSessionId { get; set; } = null!;

    public int RoundNumber { get; set; }

    public string LeaderId { get; set; } = null!;
    public string? Word { get; set; }
}
