using Microsoft.AspNetCore.SignalR;
using NotificationCalendar.Abstractions.Infrastructure;

namespace NotificationCalendar.Infrastructure.Hubs;

public class NotificationHub : Hub<INotificationSender>
{

}
