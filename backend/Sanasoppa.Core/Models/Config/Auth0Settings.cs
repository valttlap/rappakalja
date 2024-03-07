namespace Sanasoppa.Core.Models.Config;

public class Auth0Settings
{
    public string Authority { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string Domain { get; set; } = default!;
}
