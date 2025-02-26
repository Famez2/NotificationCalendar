using NotificationCalendar.Abstractions.Domain;

namespace NotificationCalendar.Domain.Options;

public class ExpiredNoteJobOptions : BaseJobOptions
{
    public const string SectionName = $"Jobs:{nameof(ExpiredNoteJobOptions)}";
}
