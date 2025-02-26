using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationCalendar.Domain.Entities;

namespace NotificationCalendar.Persistence.Configuration;

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasMany(u => u.Notes)
            .WithOne(o => o.User)
            .HasForeignKey(to => to.UserId);
    }
}
