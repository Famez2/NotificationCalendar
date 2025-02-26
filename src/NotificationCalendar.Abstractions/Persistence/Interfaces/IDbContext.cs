using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace NotificationCalendar.Abstractions.Persistence.Interfaces;

public interface IDbContext : IDisposable, IAsyncDisposable
{
    ChangeTracker ChangeTracker { get; }

    DatabaseFacade Database { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}
