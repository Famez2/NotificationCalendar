namespace NotificationCalendar.Abstractions.Application;

public interface IScvGenerateService
{
    public Task<string> GenerateCsvAsync(List<Guid> notes);
}
