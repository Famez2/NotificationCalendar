using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Abstractions.Persistence.Base;
using System.Reflection;

namespace NotificationCalendar.Persistence;

public class NotificationCalendarDbContext : BaseDbContext, INotificationCalendarDbContext
{
    public NotificationCalendarDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void ConfigureModelBuilder(ModelBuilder modelBuilder)
    {
        base.ConfigureModelBuilder(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
