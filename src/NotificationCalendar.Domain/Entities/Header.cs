using NotificationCalendar.Abstractions.Persistence.Interfaces;

namespace NotificationCalendar.Domain.Entities;

public class Header : IAuditableEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<Note> Notes { get; set; } = [];
}
