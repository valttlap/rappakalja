namespace Sanasoppa.Core.DTOs;

public class SubmissionDto
{
    public string Id { get; set; } = null!;

    public string RoundId { get; set; } = null!;

    public string PlayerId { get; set; } = null!;

    public string Guess { get; set; } = null!;
}
