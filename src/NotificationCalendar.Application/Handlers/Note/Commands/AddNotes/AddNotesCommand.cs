using MediatR;

namespace NotificationCalendar.Application.Handlers.Notes.Commands.AddNote;

public class AddNotesCommand : IRequest
{
    public Guid UserId { get; set; }

    public HeaderInfoModel Header { get; set; }

    public class NoteInfoModel
    {
        public string? Content { get; set; }

        public DateTime? ScheduledAt { get; set; }
    }

    public class HeaderInfoModel
    {
        public string? Name { get; set; }

        public List<NoteInfoModel> Notes { get; set; } = [];
    }
}
