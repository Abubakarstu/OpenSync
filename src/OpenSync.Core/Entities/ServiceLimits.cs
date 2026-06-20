namespace OpenSync.Core.Entities;

public class ServiceLimits
{
    public int MaxDocumentSizeBytes { get; set; } = 16384;
    public int MaxListItemDataSizeBytes { get; set; } = 16384;
    public int MaxMapItemDataSizeBytes { get; set; } = 16384;
    public int MaxStreamMessageSizeBytes { get; set; } = 4096;
    public int MaxListItems { get; set; } = 1000;
    public int MaxMapItems { get; set; } = 10000;
    public int MaxSubscriptionsPerConnection { get; set; } = 200;
    public int MaxConnectionsPerService { get; set; } = 10000;
    public int DefaultRateLimitPerSecond { get; set; } = 100;
}
