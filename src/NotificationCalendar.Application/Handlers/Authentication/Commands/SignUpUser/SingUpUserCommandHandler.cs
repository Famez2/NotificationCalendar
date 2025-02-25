using MediatR;
using Microsoft.Extensions.Options;
using NotificationCalendar.Domain.Options;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using NotificationCalendar.Common.Models;
using NotificationCalendar.Persistence;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using NotificationCalendar.Domain.Entities;

namespace NotificationCalendar.Application.Handlers.Authentication.Commands.SignUpUser;

public class SingUpUserCommandHandler : IRequestHandler<SingUpUserCommand>
{
    private readonly INotificationCalendarDbContext _notificationCalendarDbContext;
    private readonly IMapper _mapper;
    private readonly JwtTokenOptions _jwtTokenOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SafeCookieOptions _safeCookieOptions;

    private readonly JsonSerializerOptions SerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public SingUpUserCommandHandler(
        INotificationCalendarDbContext notificationCalendarDbContext,
        IMapper mapper,
        IOptions<JwtTokenOptions> jwtTokenOptions,
        IHttpContextAccessor httpContextAccessor,
        IOptions<SafeCookieOptions> safeCookieOptions)
    {
        _notificationCalendarDbContext = notificationCalendarDbContext;
        _mapper = mapper;
        _jwtTokenOptions = jwtTokenOptions.Value;
        _httpContextAccessor = httpContextAccessor;
        _safeCookieOptions = safeCookieOptions.Value;
    }

    public async Task Handle(SingUpUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = command.Email,
            Password = command.Password
        };

        _notificationCalendarDbContext.User.Add(user);

        await _notificationCalendarDbContext.SaveChangesAsync(cancellationToken);

        var accessToken = GenerateToken(new JwtTokenData()
        {
            Issuer = _jwtTokenOptions.Issuer,
            Audience = _jwtTokenOptions.Audience,
            ExpiresInHours = _jwtTokenOptions.ExpiresInHours,
            PrivateKey = _jwtTokenOptions.PrivateKey,
            Claims = GetClaims(user)
        });

        SetCookie(_jwtTokenOptions.TokenName, accessToken, _jwtTokenOptions.ExpiresInHours);
    }

    public string GenerateToken(JwtTokenData jwtTokenData)
    {
        var stopwatch = Stopwatch.StartNew();

        if (jwtTokenData is null)
        {
            throw new ArgumentNullException(nameof(jwtTokenData), $"{nameof(jwtTokenData)} should not be null");
        }

        try
        {
            var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Jti, jwtTokenData.Id.ToString())
                };
            claims.AddRange(jwtTokenData.Claims);

            var subject = new ClaimsIdentity(claims);

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenData.PrivateKey!)),
                SecurityAlgorithms.HmacSha256
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtTokenData.Issuer,
                Audience = jwtTokenData.Audience,
                SigningCredentials = signingCredentials,
                Subject = subject,
                IssuedAt = jwtTokenData.IssuedAtDateTimeUtc,
                NotBefore = jwtTokenData.ValidFromDateTimeUtc,
                Expires = jwtTokenData.ValidFromDateTimeUtc.AddHours(jwtTokenData.ExpiresInHours)
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var token = jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            throw;
        }
    }

    public void SetCookie(string key, string value, int? expiryInHours = null)
    {
        var cookieExpiry = expiryInHours ?? _safeCookieOptions.ExpiresInHours;

        var options = new CookieOptions
        {
            Secure = _safeCookieOptions.Secure,
            HttpOnly = _safeCookieOptions.HttpOnly,
            SameSite = _safeCookieOptions.SameSite.Value,
            Domain = _safeCookieOptions.Domain,
            Expires = DateTimeOffset.UtcNow.AddHours(cookieExpiry)
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
    }

    private List<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        return claims;
    }
}
