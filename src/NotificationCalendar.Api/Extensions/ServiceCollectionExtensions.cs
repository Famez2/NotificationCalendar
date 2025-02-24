using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Persistence;

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
        services.AddAutoMapper(typeof(Api.Mappings.MockClass).Assembly, typeof(Application.Mappings.MockClass).Assembly);
    }
}
