using MediatR;

namespace NotificationCalendar.Application.Handlers.Note.Events;

public class NoteExpiredNotification : INotification
{
    public NoteExpiredNotification(Guid id, string content)
    {
        Id = id; 
        Content = content;
    }

    public Guid Id { get; set; }

    public string Content { get; set; }
}
