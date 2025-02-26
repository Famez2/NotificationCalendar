using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationCalendar.Domain.Entities;

namespace NotificationCalendar.Persistence.Configuration;

public class NoteTypeConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasOne(n => n.Header)
            .WithMany(o => o.Notes)
            .HasForeignKey(to => to.HeaderId);

        builder.Property(n => n.ScheduledAt)
            .IsRequired();

        builder.Property(n => n.Content)
            .IsRequired();
    }
}
