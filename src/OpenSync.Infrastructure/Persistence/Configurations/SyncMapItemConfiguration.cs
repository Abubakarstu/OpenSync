using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenSync.Core.Entities;

namespace OpenSync.Infrastructure.Persistence.Configurations;

public class SyncMapItemConfiguration : IEntityTypeConfiguration<SyncMapItem>
{
    public void Configure(EntityTypeBuilder<SyncMapItem> builder)
    {
        builder.ToTable("sync_map_items");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.MapId).IsRequired();
        builder.Property(x => x.Key).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Data).IsRequired().HasColumnType("jsonb");
        builder.Property(x => x.Revision).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.ExpiresAt);
        builder.HasIndex(x => new { x.MapId, x.Key }).IsUnique();
        builder.HasIndex(x => x.ExpiresAt).HasFilter("\"expires_at\" IS NOT NULL");
    }
}
