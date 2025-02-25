using FluentValidation;

namespace NotificationCalendar.Application.Handlers.Notes.Commands.AddNote;

public class AddNotesCommandValidator : AbstractValidator<AddNotesCommand>
{
    public AddNotesCommandValidator()
    {
        RuleFor(c => c.Header.Name)
            .NotEmpty()
            .WithMessage("Имя заголовка не должно быть пустым");

        RuleForEach(c => c.Header.Notes)
            .ChildRules(notes =>
            {
                notes.RuleFor(n => n.Content)
                    .NotEmpty()
                    .WithMessage("Содержимое заметки не должно быть пустым");

                notes.RuleFor(n => n.ScheduledAt)
                    .NotEmpty()
                    .WithMessage("Дата и время заметки не должны быть пустыми");
            });
    }
}
