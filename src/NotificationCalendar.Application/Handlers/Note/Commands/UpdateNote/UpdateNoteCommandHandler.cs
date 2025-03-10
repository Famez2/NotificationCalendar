﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Common.Exceptions;
using NotificationCalendar.Persistence;

namespace NotificationCalendar.Application.Handlers.Note.Commands.UpdateNote;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand>
{
    private readonly INotificationCalendarDbContext _notificationCalendarDbContext;

    public UpdateNoteCommandHandler(INotificationCalendarDbContext notificationCalendarDbContext)
    {
        _notificationCalendarDbContext = notificationCalendarDbContext;
    }

    public async Task Handle(UpdateNoteCommand command, CancellationToken cancellationToken)
    {
        var note = await _notificationCalendarDbContext.Note.FirstOrDefaultAsync(n => n.Id == command.Id, cancellationToken);

        if (note == null)
        {
            throw new BadRequestException("Заметка не найдена");
        }

        note.Content = command.Content;

        if (command.ScheduledAt != null)
        {
            note.ScheduledAt = command.ScheduledAt.Value;
        }

        await _notificationCalendarDbContext.SaveChangesAsync(cancellationToken);
    }
}
