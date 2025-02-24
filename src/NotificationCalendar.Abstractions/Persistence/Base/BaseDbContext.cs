using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotificationCalendar.Abstractions.Persistence.Interfaces;

namespace NotificationCalendar.Abstractions.Persistence.Base;

public abstract class BaseDbContext : DbContext, IDbContext
{
    protected BaseDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        SetupDateTimeUtcConvertion(modelBuilder);

        ConfigureModelBuilder(modelBuilder);
    }

    protected virtual void ConfigureModelBuilder(ModelBuilder modelBuilder)
    {
    }

    private void SetupDateTimeUtcConvertion(ModelBuilder modelBuilder)
    {
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless)
            {
                continue;
            }

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }
    }

    private void OnBeforeSaving()
    {
        SaveTimestampEntityUpdates();
    }

    private void SaveTimestampEntityUpdates()
    {
        var utcNow = DateTime.UtcNow;

        var entries = ChangeTracker.Entries()
            .Where(x => x.Entity is IAuditableEntity && (x.State is EntityState.Added or EntityState.Modified))
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.CurrentValues[nameof(IAuditableEntity.CreatedAt)] = utcNow;
                entry.CurrentValues[nameof(IAuditableEntity.UpdatedAt)] = utcNow;

                continue;
            }

            var isManualUpdate = (DateTime?)entry.CurrentValues[nameof(IAuditableEntity.UpdatedAt)]
                != (DateTime?)entry.OriginalValues[nameof(IAuditableEntity.UpdatedAt)];

            if (!isManualUpdate)
            {
                entry.CurrentValues[nameof(IAuditableEntity.UpdatedAt)] = utcNow;
            }
        }
    }
}
