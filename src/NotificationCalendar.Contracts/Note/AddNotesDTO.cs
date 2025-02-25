namespace NotificationCalendar.Contracts.Note;

public class AddNotesDTO
{
    public HeaderInfoModel Header {  get; set; }

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
