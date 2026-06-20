using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenSync.Core.Entities;

namespace OpenSync.Infrastructure.Persistence.Configurations;

public class ChannelMemberConfiguration : IEntityTypeConfiguration<ChannelMember>
{
    public void Configure(EntityTypeBuilder<ChannelMember> builder)
    {
        builder.ToTable("channel_members");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ChannelId).IsRequired();
        builder.Property(x => x.Identity).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Metadata).HasColumnType("jsonb");
        builder.Property(x => x.LastSeenAt).IsRequired();
        builder.HasIndex(x => new { x.ChannelId, x.Identity }).IsUnique();
    }
}
