using FluentValidation;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using NotificationCalendar.Abstractions.Application;
using NotificationCalendar.Api.Behaviors;
using NotificationCalendar.Api.Extensions;
using NotificationCalendar.Api.Middleware;
using NotificationCalendar.Application.Handlers;
using NotificationCalendar.Application.Handlers.Notes.Commands.AddNote;
using NotificationCalendar.Application.Services;
using NotificationCalendar.Domain.Entities;
using NotificationCalendar.Domain.Options;
using NotificationCalendar.Infrastructure.Hubs;
using Serilog;
using System;

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
        services.AddControllers().AddOData(opt => 
        opt.AddRouteComponents("odata", GetEdmModel())
            .Filter()
            .Select()
            .Expand()
            .Count()
            .OrderBy());

        services.AddJwtTokenAuth(Configuration);

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(AddNotesCommand).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(AddNotesCommand).Assembly);

        services.AddQuartz(Configuration);

        services.AddScoped<IScvGenerateService, ScvGenerateService>();

        services.ConfigureAutoMapper(Configuration);

        services.AddCors();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseSwaggerDocumentation();

        app.UseSerilogRequestLogging();

        app.UseODataBatching();
        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<NotificationHub>("/api/signalr");
        });
    }

    private IEdmModel GetEdmModel()
    {
        var odataBuilder = new ODataConventionModelBuilder();
        odataBuilder.EntitySet<Note>("notes");
        odataBuilder.EntitySet<Header>("Header");

        return odataBuilder.GetEdmModel();
    }
}
