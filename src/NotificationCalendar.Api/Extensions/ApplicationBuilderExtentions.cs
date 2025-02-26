namespace NotificationCalendar.Api.Extensions;

public static class ApplicationBuilderExtentions
{
    public static void UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger(opt => opt.RouteTemplate = "api/swagger/{documentName}/swagger.json");
        app.UseSwaggerUI(opt => opt.RoutePrefix = "api/swagger");
    }
}
