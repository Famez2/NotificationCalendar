using MediatR;
using Microsoft.AspNetCore.SignalR;
using NotificationCalendar.Abstractions.Infrastructure;
using NotificationCalendar.Infrastructure.Hubs;

namespace NotificationCalendar.Application.Handlers.Note.Events;

public class NoteExpiredNotificationHandler : INotificationHandler<NoteExpiredNotification>
{
    private readonly IHubContext<NotificationHub, INotificationSender> _hubContext;

    public NoteExpiredNotificationHandler(IHubContext<NotificationHub, INotificationSender> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(NoteExpiredNotification notification, CancellationToken cancellationToken)
    {
        await _hubContext
            .Clients
            .User(notification.Id.ToString())
            .NoteExpired(notification.Content);
    }
}
