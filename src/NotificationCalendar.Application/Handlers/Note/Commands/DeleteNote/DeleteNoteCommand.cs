using MediatR;

namespace NotificationCalendar.Application.Handlers.Note.Commands.DeleteNote;

public class DeleteNoteCommand : IRequest
{
    public Guid Id { get; set; }
}
