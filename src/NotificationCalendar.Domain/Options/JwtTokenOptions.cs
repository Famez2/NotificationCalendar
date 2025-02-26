namespace NotificationCalendar.Domain.Options;

public class JwtTokenOptions 
{
    public const string SectionName = nameof(JwtTokenOptions);

    public required string TokenName { get; set; }

    public string? Issuer { get; set; }

    public string? Audience { get; set; }

    public required string PrivateKey { get; set; }

    public required int ExpiresInHours { get; set; }

    public required string BlackListPrefix { get; set; }
}
