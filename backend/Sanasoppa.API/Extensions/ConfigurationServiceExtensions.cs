using Sanasoppa.Core.Models.Config;

namespace Sanasoppa.API.Extensions;

public static class ConfigurationServiceExtensions
{
    public static IServiceCollection AddConfigurationsServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<Auth0Settings>(config.GetSection("Auth0"));
        return services;
    }

}
