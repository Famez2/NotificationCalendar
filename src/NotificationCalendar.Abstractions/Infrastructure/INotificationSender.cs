using Microsoft.AspNetCore.SignalR;

namespace NotificationCalendar.Abstractions.Infrastructure;

public interface INotificationSender
{
    [HubMethodName("noneExpired")]
    public Task NoteExpired(string Content);
}
