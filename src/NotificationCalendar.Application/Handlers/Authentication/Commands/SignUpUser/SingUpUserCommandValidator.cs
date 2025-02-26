using FluentValidation;

namespace NotificationCalendar.Application.Handlers.Authentication.Commands.SignUpUser;

public class SingUpUserCommandValidator : AbstractValidator<SingUpUserCommand>
{
    public SingUpUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email не должен быть пустым");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Пароль не должен быть пустым");
    }
}
