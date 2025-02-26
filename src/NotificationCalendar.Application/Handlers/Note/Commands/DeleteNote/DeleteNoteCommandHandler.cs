using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Common.Exceptions;
using NotificationCalendar.Persistence;

namespace NotificationCalendar.Application.Handlers.Note.Commands.DeleteNote;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand>
{
    private readonly INotificationCalendarDbContext _notificationCalendarDbContext;

    public DeleteNoteCommandHandler(INotificationCalendarDbContext notificationCalendarDbContext)
    {
        _notificationCalendarDbContext = notificationCalendarDbContext;
    }

    public async Task Handle(DeleteNoteCommand command, CancellationToken cancellationToken)
    {
        var note = await _notificationCalendarDbContext.Note.FirstOrDefaultAsync(n => n.Id == command.Id, cancellationToken);

        if (note == null)
        {
            throw new BadRequestException("Заметка не найдена");
        }

        _notificationCalendarDbContext.Note.Remove(note);

        await _notificationCalendarDbContext.SaveChangesAsync(cancellationToken);
    }
}
