using Microsoft.OpenApi.Models;
using Sanasoppa.API.Exceptions;

namespace Sanasoppa.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(config["ClientUrl"] ?? throw new ConfigurationException("ClientUrl is not configured"))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
            });
        });

        services.AddOpenApiDocument(config =>
        {
            config.PostProcess = document =>
            {
                document.Info.Version = "v1";
                document.Info.Title = "Sanasoppa API";
                document.Info.Description = "API for Sanasoppa";
            };
        });

        return services;
    }
}
