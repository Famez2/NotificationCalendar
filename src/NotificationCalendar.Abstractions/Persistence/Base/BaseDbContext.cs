using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Abstractions.Persistence.Interfaces;

namespace NotificationCalendar.Abstractions.Persistence.Base;

public class BaseDbContext : DbContext, IDbContext
{
    protected BaseDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureModelBuilder(modelBuilder);
    }

    protected virtual void ConfigureModelBuilder(ModelBuilder modelBuilder)
    {
    }
}
