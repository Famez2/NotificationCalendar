using NotificationCalendar.Api.Hosting;

namespace NotificationCalendar.Api;

public class Program
{
    public static void Main(string[] args)
    {
        ApiHost.Run<Startup>(args);
    }
}
