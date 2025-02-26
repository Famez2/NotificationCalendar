using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Abstractions.Application;
using NotificationCalendar.Persistence;
using System.Text;

namespace NotificationCalendar.Application.Services;

public class ScvGenerateService : IScvGenerateService
{
    private readonly INotificationCalendarDbContext _notificationCalendarDbContext;

    public ScvGenerateService(
        INotificationCalendarDbContext notificationCalendarDbContext,)
    {
        _notificationCalendarDbContext = notificationCalendarDbContext;
    }

    public async Task<string> GenerateCsvAsync(List<Guid> noteIds)
    {
        var notes = await _notificationCalendarDbContext.Note
            .Include(n => n.Header)
            .Where(n => noteIds.Contains(n.Id))
            .ToListAsync();

        var sb = new StringBuilder();

        sb.AppendLine("Id,Content,HeaderId,UserId,ScheduledAt,CreatedAt,UpdatedAt");

        foreach (var note in notes)
        {
            sb.AppendLine($"{note.Id},{note.Content},{note.HeaderId},{note.UserId},{note.ScheduledAt},{note.CreatedAt},{note.UpdatedAt}");
        }

        return sb.ToString();
    }
}
