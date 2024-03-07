using System.ComponentModel.DataAnnotations;

namespace Sanasoppa.Core.DTOs;

public class SettingsDto
{
    [Required]
    public string Domain { get; set; } = null!;

    [Required]
    public string ClientId { get; set; } = null!;

    [Required]
    public string Audience { get; set; } = null!;
}
