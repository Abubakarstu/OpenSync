using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenSync.Core.Entities;

namespace OpenSync.Infrastructure.Persistence.Configurations;

public class SyncDocumentConfiguration : IEntityTypeConfiguration<SyncDocument>
{
    public void Configure(EntityTypeBuilder<SyncDocument> builder)
    {
        builder.ToTable("sync_documents");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ServiceId).IsRequired();
        builder.Property(x => x.UniqueName).HasMaxLength(256);
        builder.Property(x => x.Data).IsRequired().HasColumnType("jsonb");
        builder.Property(x => x.Revision).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.ExpiresAt);
        builder.HasIndex(x => new { x.ServiceId, x.UniqueName }).IsUnique().HasFilter("\"unique_name\" IS NOT NULL");
        builder.HasIndex(x => x.ExpiresAt).HasFilter("\"expires_at\" IS NOT NULL");
    }
}
