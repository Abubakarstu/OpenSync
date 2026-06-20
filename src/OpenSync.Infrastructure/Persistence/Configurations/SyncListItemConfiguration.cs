using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenSync.Core.Entities;

namespace OpenSync.Infrastructure.Persistence.Configurations;

public class SyncListItemConfiguration : IEntityTypeConfiguration<SyncListItem>
{
    public void Configure(EntityTypeBuilder<SyncListItem> builder)
    {
        builder.ToTable("sync_list_items");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ListId).IsRequired();
        builder.Property(x => x.Index).IsRequired();
        builder.Property(x => x.Data).IsRequired().HasColumnType("jsonb");
        builder.Property(x => x.Revision).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.ExpiresAt);
        builder.HasIndex(x => new { x.ListId, x.Index });
        builder.HasIndex(x => x.ExpiresAt).HasFilter("\"expires_at\" IS NOT NULL");
    }
}
