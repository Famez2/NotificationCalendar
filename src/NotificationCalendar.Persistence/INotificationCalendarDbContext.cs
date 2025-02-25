using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Abstractions.Persistence.Interfaces;
using NotificationCalendar.Domain.Entities;

namespace NotificationCalendar.Persistence;

public interface INotificationCalendarDbContext : IDbContext
{
    public DbSet<Note> Note { get; set; }

    public DbSet<Header> Header { get; set; }

    public DbSet<User> User { get; set; }
}
