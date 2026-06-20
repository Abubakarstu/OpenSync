namespace OpenSync.Infrastructure.Realtime.Connection;

public class ConnectionInfo
{
    public string ConnectionId { get; set; } = string.Empty;
    public string Identity { get; set; } = string.Empty;
    public string TransportType { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; }
    public DateTime LastHeartbeat { get; set; }
    public bool IsActive { get; set; }
}
