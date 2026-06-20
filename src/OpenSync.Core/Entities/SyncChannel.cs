namespace OpenSync.Core.Entities;

public class SyncChannel : BaseEntity
{
    public Guid ServiceId { get; private set; }
    public string? UniqueName { get; private set; }
    public ChannelAttributes? Attributes { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public int MemberCount { get; private set; }

    private SyncChannel() { }

    public SyncChannel(Guid serviceId, string? uniqueName = null, ChannelAttributes? attributes = null, DateTime? expiresAt = null)
    {
        ServiceId = serviceId;
        UniqueName = uniqueName;
        Attributes = attributes ?? new ChannelAttributes();
        ExpiresAt = expiresAt;
        MemberCount = 0;
    }

    public void IncrementMemberCount() { MemberCount++; UpdatedAt = DateTime.UtcNow; }
    public void DecrementMemberCount() { if (MemberCount > 0) MemberCount--; UpdatedAt = DateTime.UtcNow; }
    public void UpdateAttributes(ChannelAttributes attributes) { Attributes = attributes; UpdatedAt = DateTime.UtcNow; }
}
