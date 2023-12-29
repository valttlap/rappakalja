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
    public string? Word { get; set; }
    public IEnumerable<SubmissionReturnDto> Submissions = new List<SubmissionReturnDto>();
}
