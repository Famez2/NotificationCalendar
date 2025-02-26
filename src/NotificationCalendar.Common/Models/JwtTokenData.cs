using System.Security.Claims;

namespace NotificationCalendar.Common.Models;

public class JwtTokenData
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Issuer { get; set; }

    public string? Audience { get; set; }

    public string? PrivateKey { get; set; }

    public int ExpiresInHours { get; set; }

    public DateTime IssuedAtDateTimeUtc { get; set; } = DateTime.UtcNow;

    public DateTime ValidFromDateTimeUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Claim> Claims { get; set; } = [];
}
