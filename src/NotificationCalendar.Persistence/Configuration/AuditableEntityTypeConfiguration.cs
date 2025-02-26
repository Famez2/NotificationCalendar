using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Abstractions.Persistence.Interfaces;

namespace Absplan.Persistence.Configurations;

public class AuditableEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IAuditableEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .Property(b => b.CreatedAt)
            .HasColumnType("timestamp without time zone");

        builder
            .Property(b => b.UpdatedAt)
            .HasColumnType("timestamp without time zone");
    }
}

