namespace OpenSync.Api.Models.Responses;

public class StatsResponse
{
    public int ConnectionCount { get; set; }
    public int SubscriptionCount { get; set; }
    public int DocumentCount { get; set; }
    public int ListCount { get; set; }
    public int MapCount { get; set; }
    public int StreamCount { get; set; }
    public int ChannelCount { get; set; }
    public int TotalObjectCount { get; set; }
}
