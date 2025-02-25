using FluentValidation;

namespace NotificationCalendar.Application.Handlers.Note.Commands.UpdateNote;

public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator()
    {
        RuleFor(c => c.Content)
            .NotEmpty()
            .WithMessage("Содержание заметки не должно быть пустым");
    }
}
