using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenSync.Core.Entities;

namespace OpenSync.Infrastructure.Persistence.Configurations;

public class SyncStreamConfiguration : IEntityTypeConfiguration<SyncStream>
{
    public void Configure(EntityTypeBuilder<SyncStream> builder)
    {
        builder.ToTable("sync_streams");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ServiceId).IsRequired();
        builder.Property(x => x.UniqueName).HasMaxLength(256);
        builder.Property(x => x.ExpiresAt);
        builder.HasIndex(x => new { x.ServiceId, x.UniqueName }).IsUnique().HasFilter("\"unique_name\" IS NOT NULL");
    }
}
