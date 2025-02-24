using NotificationCalendar.Abstractions.Persistence.Interfaces;

namespace NotificationCalendar.Domain.Entities;

public class Note : IAuditableEntity
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public Guid HeaderId { get; set; }

    public DateTime ScheduledAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Header Header { get; set; }
}
