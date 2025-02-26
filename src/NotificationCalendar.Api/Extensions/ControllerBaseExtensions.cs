using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NotificationCalendar.Api.Extensions;

public static class ControllerBaseExtensions
{
    public static Guid GetUserId(this ControllerBase controller)
    {
        var isValidId = Guid.TryParse(
            controller.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out Guid userId);

        if (!isValidId || userId == Guid.Empty)
        {
            throw new ArgumentException("Cannot get claim {ClaimType} to get user identifier", ClaimTypes.NameIdentifier);
        }

        return userId;
    }
}
