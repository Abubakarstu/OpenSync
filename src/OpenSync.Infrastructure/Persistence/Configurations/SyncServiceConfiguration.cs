using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenSync.Core.Entities;

namespace OpenSync.Infrastructure.Persistence.Configurations;

public class SyncServiceConfiguration : IEntityTypeConfiguration<SyncService>
{
    public void Configure(EntityTypeBuilder<SyncService> builder)
    {
        builder.ToTable("sync_services");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
        builder.Property(x => x.Description).HasMaxLength(512);
        builder.Property(x => x.WebhookUrl).HasMaxLength(2048);
        builder.Property(x => x.WebhookSecret).HasMaxLength(256);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        builder.OwnsOne(x => x.Limits, limits =>
        {
            limits.Property(l => l.MaxDocumentSizeBytes).HasColumnName("max_document_size_bytes").HasDefaultValue(16384);
            limits.Property(l => l.MaxListItemDataSizeBytes).HasColumnName("max_list_item_data_size_bytes").HasDefaultValue(16384);
            limits.Property(l => l.MaxMapItemDataSizeBytes).HasColumnName("max_map_item_data_size_bytes").HasDefaultValue(16384);
            limits.Property(l => l.MaxStreamMessageSizeBytes).HasColumnName("max_stream_message_size_bytes").HasDefaultValue(4096);
            limits.Property(l => l.MaxListItems).HasColumnName("max_list_items").HasDefaultValue(1000);
            limits.Property(l => l.MaxMapItems).HasColumnName("max_map_items").HasDefaultValue(10000);
            limits.Property(l => l.MaxSubscriptionsPerConnection).HasColumnName("max_subscriptions_per_connection").HasDefaultValue(200);
            limits.Property(l => l.MaxConnectionsPerService).HasColumnName("max_connections_per_service").HasDefaultValue(10000);
            limits.Property(l => l.DefaultRateLimitPerSecond).HasColumnName("default_rate_limit_per_second").HasDefaultValue(100);
        });
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
