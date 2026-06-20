using OpenSync.Core.ValueObjects;

namespace OpenSync.Core.Entities;

public class ChannelMember : BaseEntity
{
    public Guid ChannelId { get; private set; }
    public string Identity { get; private set; } = null!;
    public JsonData? Metadata { get; private set; }
    public DateTime LastSeenAt { get; private set; }

    private ChannelMember() { }

    public ChannelMember(Guid channelId, string identity, JsonData? metadata = null)
    {
        ChannelId = channelId;
        Identity = identity ?? throw new ArgumentNullException(nameof(identity));
        Metadata = metadata;
        LastSeenAt = DateTime.UtcNow;
    }

    public void UpdateMetadata(JsonData? metadata)
    {
        Metadata = metadata;
        LastSeenAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Touch()
    {
        LastSeenAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
