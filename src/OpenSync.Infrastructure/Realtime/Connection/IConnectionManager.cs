using OpenSync.Core.Enums;

namespace OpenSync.Infrastructure.Realtime.Connection;

public interface IConnectionManager
{
    string AddConnection(string identity, TransportType transportType);
    bool RemoveConnection(string connectionId);
    ConnectionSession? GetConnection(string connectionId);
    IReadOnlyList<ConnectionSession> GetAllConnections();
    IReadOnlyList<ConnectionSession> GetConnectionsByIdentity(string identity);
    int ConnectionCount { get; }
    bool UpdateHeartbeat(string connectionId);
    bool SetState(string connectionId, ConnectionState state);
}
