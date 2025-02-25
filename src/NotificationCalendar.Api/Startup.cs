using NotificationCalendar.Api.Extensions;
using NotificationCalendar.Api.Middleware;
using NotificationCalendar.Application.Handlers;
using NotificationCalendar.Application.Handlers.Note.Commands.AddNote;
using Serilog;

namespace NotificationCalendar.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSwaggerDocs(Configuration);

        services.AddControllers();

        services.AddNotificationCalendarDbContext(Configuration);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddNotesCommandHandler).Assembly));

        services.ConfigureAutoMapper(Configuration);

        services.AddCors();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseSwaggerDocumentation();

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
