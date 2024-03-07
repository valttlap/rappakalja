using Microsoft.Extensions.Options;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Models.Config;

namespace Sanasoppa.Core.Services;

public class SettingsService
{
    private readonly IOptions<Auth0Settings> _settings;

    public SettingsService(IOptions<Auth0Settings> settings)
    {
        _settings = settings;
    }

    public SettingsDto GetSettings()
    {
        return new SettingsDto
        {
            Domain = _settings.Value.Domain,
            ClientId = _settings.Value.ClientId,
            Audience = _settings.Value.Audience
        };
    }
}
