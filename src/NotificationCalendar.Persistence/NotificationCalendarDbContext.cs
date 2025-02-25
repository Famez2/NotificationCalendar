using Absplan.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Abstractions.Persistence.Base;
using NotificationCalendar.Domain.Entities;
using System.Reflection;

namespace NotificationCalendar.Persistence;

public class NotificationCalendarDbContext : BaseDbContext, INotificationCalendarDbContext
{
    public NotificationCalendarDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Note> Note { get; set; }

    public DbSet<Header> Header { get; set; }

    public DbSet<User> User { get; set; }

    protected override void ConfigureModelBuilder(ModelBuilder modelBuilder)
    {
        base.ConfigureModelBuilder(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.ApplyConfiguration(new AuditableEntityTypeConfiguration<Note>());
        modelBuilder.ApplyConfiguration(new AuditableEntityTypeConfiguration<Header>());
    }
}
