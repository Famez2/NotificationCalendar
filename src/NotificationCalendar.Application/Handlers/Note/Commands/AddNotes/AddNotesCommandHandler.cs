using MediatR;
using NotificationCalendar.Persistence;

namespace NotificationCalendar.Application.Handlers.Notes.Commands.AddNote;

public class AddNotesCommandHandler : IRequestHandler<AddNotesCommand>
{
    private readonly INotificationCalendarDbContext _notificationCalendarDbContext;

    public AddNotesCommandHandler(INotificationCalendarDbContext notificationCalendarDbContext)
    {
        _notificationCalendarDbContext = notificationCalendarDbContext;
    }

    public async Task Handle(AddNotesCommand command, CancellationToken cancellationToken)
    {
        var header = new Domain.Entities.Header
        {
            Name = command.Header.Name,
            Notes = command.Header.Notes
            .Select(n => new Domain.Entities.Note
            {
                Content = n.Content,
                ScheduledAt = n.ScheduledAt.Value,
                UserId = command.UserId
            })
            .ToList()
        };

        _notificationCalendarDbContext.Header.Add(header);

        await _notificationCalendarDbContext.SaveChangesAsync(cancellationToken);
    }
}
