using OpenSync.Core.Enums;

namespace OpenSync.Infrastructure.Realtime.Connection;

public class ConnectionSession
{
    public string ConnectionId { get; }
    public string Identity { get; }
    public TransportType TransportType { get; }
    public ConnectionState State { get; set; }
    public DateTime ConnectedAt { get; }
    public DateTime LastHeartbeat { get; set; }
    public Dictionary<string, object> Metadata { get; } = new();

    public ConnectionSession(string connectionId, string identity, TransportType transportType)
    {
        ConnectionId = connectionId;
        Identity = identity;
        TransportType = transportType;
        State = ConnectionState.Connected;
        ConnectedAt = DateTime.UtcNow;
        LastHeartbeat = DateTime.UtcNow;
    }
}
