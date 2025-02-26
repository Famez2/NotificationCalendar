using MediatR;

namespace NotificationCalendar.Application.Handlers.Authentication.Commands.SignUpUser;

public class SingUpUserCommand : IRequest
{
    public string? Email { get; set; }

    public string? Password { get; set; }
}
