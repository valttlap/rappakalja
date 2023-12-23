namespace Sanasoppa.API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
    {
        /* services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var settings = config.GetSection("Auth0").Get<Auth0Settings>() ?? throw new ConfigurationException("Auth0 settings not found");
            options.Authority = settings.Authority;
            options.Audience = settings.Audience;
            options.RequireHttpsMetadata = !env.IsDevelopment();
        }); */

        return services;
    }
}
