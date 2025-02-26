namespace NotificationCalendar.Contracts.Note;

public class UpdateNoteDTO
{
    public string? Content { get; set; }

    public DateTime? ScheduledAt { get; set; }
}
