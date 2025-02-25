using Microsoft.AspNetCore.Http;

namespace NotificationCalendar.Domain.Options;

public class SafeCookieOptions
{
    public const string SectionName = nameof(SafeCookieOptions);

    public bool Secure { get; set; }

    public string Domain { get; set; }

    public bool HttpOnly { get; set; }

    public int ExpiresInHours { get; set; }

    public SameSiteMode? SameSite { get; set; }
}
