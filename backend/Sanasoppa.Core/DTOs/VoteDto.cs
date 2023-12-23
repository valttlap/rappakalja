namespace Sanasoppa.Core.DTOs;

public class VoteDto
{
    public string Id { get; set; } = null!;

    public string RoundId { get; set; } = null!;

    public string VoterId { get; set; } = null!;

    public string SubmissionId { get; set; } = null!;
}
