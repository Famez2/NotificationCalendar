using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Persistence;
using System.Reflection;

namespace NotificationCalendar.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSwaggerDocs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            c.CustomSchemaIds(type => type.ToString()
                .Replace("+", "_")
                .Replace("`1", ""));
        });
    }

    public static void ConfigureAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Api.Mappings.MockClass).Assembly, typeof(Application.Mappings.MockClass).Assembly);
    }

    public static void AddNotificationCalendarDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<INotificationCalendarDbContext, NotificationCalendarDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("NotificationCalendarDatabase")));
    }
}
