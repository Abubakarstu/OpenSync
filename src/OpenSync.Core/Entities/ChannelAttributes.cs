namespace OpenSync.Core.Entities;

public class ChannelAttributes
{
    public string? Type { get; set; }
    public bool IsPrivate { get; set; }
    public int MaxMembers { get; set; }
    public Dictionary<string, string> Custom { get; set; } = new();

    public ChannelAttributes()
    {
        MaxMembers = 1000;
    }
}
