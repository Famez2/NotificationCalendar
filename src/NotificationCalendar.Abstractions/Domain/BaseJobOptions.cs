namespace NotificationCalendar.Abstractions.Domain;

public class BaseJobOptions
{
    public string Identity { get; set; }

    public string IdentityTrigger { get; set; }

    public string Cron { get; set; }

    protected BaseJobOptions()
    {
        Identity = this.GetType().Name;
        IdentityTrigger = $"{Identity}-trigger";
    }
}
