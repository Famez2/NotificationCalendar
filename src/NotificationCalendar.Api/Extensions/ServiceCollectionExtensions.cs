using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotificationCalendar.Domain.Options;
using NotificationCalendar.Persistence;
using System.Security.AccessControl;
using System.Text;

namespace NotificationCalendar.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddNotificationCalendarDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<INotificationCalendarDbContext, NotificationCalendarDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("NotificationCalendarDatabase")));
    }

    public static void AddSwaggerDocs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void ConfigureAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Api.Mappings.NoteMappingProfile).Assembly, typeof(Application.Mappings.MockClass).Assembly);
    }

    public static IServiceCollection AddJwtTokenAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration
                             .GetSection(JwtTokenOptions.SectionName)
                             .Get<JwtTokenOptions>()
                         ?? throw new ArgumentNullException($"Options object for '{typeof(JwtTokenOptions)}' is null.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.PrivateKey)
                ),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
            options.SaveToken = true;
            options.Events = new()
            {
                OnMessageReceived = context =>
                {
                    var request = context.HttpContext.Request;
                    var cookies = request.Cookies;

                    if (cookies.TryGetValue(jwtOptions.TokenName, out var accessTokenValue))
                    {
                        context.Token = accessTokenValue;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}
