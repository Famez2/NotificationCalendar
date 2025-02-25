using FluentValidation;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using NotificationCalendar.Api.Behaviors;
using NotificationCalendar.Api.Extensions;
using NotificationCalendar.Api.Middleware;
using NotificationCalendar.Application.Handlers;
using NotificationCalendar.Application.Handlers.Notes.Commands.AddNote;
using NotificationCalendar.Domain.Options;
using NotificationCalendar.Infrastructure.Hubs;
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
        services.Configure<JwtTokenOptions>(Configuration.GetSection(JwtTokenOptions.SectionName));
        services.Configure<SafeCookieOptions>(Configuration.GetSection(SafeCookieOptions.SectionName));
        services.Configure<ExpiredNoteJobOptions>(Configuration.GetSection(ExpiredNoteJobOptions.SectionName));

        services.AddHttpContextAccessor();
        services.AddSwaggerDocs(Configuration);

        services.AddSignalR();

        services.AddControllers();

        services.AddNotificationCalendarDbContext(Configuration);

        services.AddJwtTokenAuth(Configuration);

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(AddNotesCommand).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(AddNotesCommand).Assembly);

        services.AddQuartz(Configuration);

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
            endpoints.MapHub<NotificationHub>("/api/signalr");
        });
    }
}
