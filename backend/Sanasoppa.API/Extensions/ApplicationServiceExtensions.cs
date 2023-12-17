using Sanasoppa.API.Exceptions;
using Sanasoppa.Core.Helpers;
using Sanasoppa.Core.Repositories;
using Sanasoppa.Core.Services;

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

        services.AddSignalR();
        services.AddAutoMapper(cfg => cfg.AddProfile(typeof(AutomapperProfiles)), AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<UnitOfWork>();
        services.AddScoped<GameService>();
        services.AddScoped<PlayerService>();
        services.AddScoped<RoundService>();

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
