using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenSync.Core.Entities;

namespace OpenSync.Infrastructure.Persistence.Configurations;

public class SyncChannelConfiguration : IEntityTypeConfiguration<SyncChannel>
{
    public void Configure(EntityTypeBuilder<SyncChannel> builder)
    {
        builder.ToTable("sync_channels");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ServiceId).IsRequired();
        builder.Property(x => x.UniqueName).HasMaxLength(256);
        builder.Property(x => x.MemberCount).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.ExpiresAt);
        builder.OwnsOne(x => x.Attributes, attr =>
        {
            attr.Property(a => a.Type).HasColumnName("channel_type").HasMaxLength(64);
            attr.Property(a => a.IsPrivate).HasColumnName("is_private").HasDefaultValue(false);
            attr.Property(a => a.MaxMembers).HasColumnName("max_members").HasDefaultValue(1000);
            attr.Property(a => a.Custom).HasColumnName("custom_attributes").HasColumnType("jsonb").HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());
        });
        builder.HasIndex(x => new { x.ServiceId, x.UniqueName }).IsUnique().HasFilter("\"unique_name\" IS NOT NULL");
    }
}
