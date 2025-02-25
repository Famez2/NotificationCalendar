using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Application.Handlers.Note.Events;
using NotificationCalendar.Persistence;
using Quartz;

namespace NotificationCalendar.Application.Jobs;

public class NoteExpiredJob : IJob
{
    private readonly INotificationCalendarDbContext _notificationCalendarDbContext;
    private readonly IPublisher _publisher;

    public NoteExpiredJob(
        INotificationCalendarDbContext notificationCalendarDbContext,
        IPublisher publisher)
    {
        _notificationCalendarDbContext = notificationCalendarDbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var notes = await _notificationCalendarDbContext.Note
            .Where(n => n.ScheduledAt.Date == DateTime.UtcNow.Date
                && n.ScheduledAt.Hour == DateTime.UtcNow.Hour
                && n.ScheduledAt.Minute == DateTime.UtcNow.Minute)
            .ToListAsync(context.CancellationToken);

        foreach (var note in notes)
        {
            await _publisher.Publish(new NoteExpiredNotification(note.UserId, note.Content), context.CancellationToken);
        }
    }
}
