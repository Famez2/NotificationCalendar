using MediatR;

namespace NotificationCalendar.Application.Handlers.Note.Commands.UpdateNote;

public class UpdateNoteCommand : IRequest
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public DateTime? ScheduledAt { get; set; }
}
