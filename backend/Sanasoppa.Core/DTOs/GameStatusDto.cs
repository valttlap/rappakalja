using System.ComponentModel.DataAnnotations;

namespace Sanasoppa.Core.DTOs;

public enum GameState
{
    Wait,
    SubmitWord,
    SubmitGuess,
    ReadGuesses
}

public class GameStatusDto
{
    [Required]
    public GameState State { get; set; }
    public bool IsOwner { get; set; }
    public bool IsDasher { get; set; }
    public string GameId { get; set; } = null!;
    public string JoinCode { get; set; } = null!;
    public string? Word { get; set; }
    public IEnumerable<SubmissionReturnDto> Submissions { get; set; } = new List<SubmissionReturnDto>();
}
