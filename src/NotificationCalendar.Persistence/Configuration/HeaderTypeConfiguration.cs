using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationCalendar.Domain.Entities;

namespace NotificationCalendar.Persistence.Configuration;

public class HeaderTypeConfiguration : IEntityTypeConfiguration<Header>
{
    public void Configure(EntityTypeBuilder<Header> builder)
    {
        builder.HasKey(h => h.Id);
    }
}
