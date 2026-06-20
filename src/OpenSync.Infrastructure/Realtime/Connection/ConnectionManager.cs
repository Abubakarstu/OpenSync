using OpenSync.Core.Enums;
using System.Collections.Concurrent;

namespace OpenSync.Infrastructure.Realtime.Connection;

public class ConnectionManager : IConnectionManager
{
    private readonly ConcurrentDictionary<string, ConnectionSession> _connections = new();
    private readonly ConcurrentDictionary<string, HashSet<string>> _identityConnections = new();

    public string AddConnection(string identity, TransportType transportType)
    {
        var connectionId = Guid.NewGuid().ToString("N");
        var session = new ConnectionSession(connectionId, identity, transportType);

        _connections[connectionId] = session;
        _identityConnections.AddOrUpdate(
            identity,
            _ => new HashSet<string> { connectionId },
            (_, set) => { set.Add(connectionId); return set; });

        return connectionId;
    }

    public bool RemoveConnection(string connectionId)
    {
        if (_connections.TryRemove(connectionId, out var session))
        {
            if (_identityConnections.TryGetValue(session.Identity, out var set))
            {
                set.Remove(connectionId);
                if (set.Count == 0)
                    _identityConnections.TryRemove(session.Identity, out _);
            }
            return true;
        }
        return false;
    }

    public ConnectionSession? GetConnection(string connectionId)
    {
        _connections.TryGetValue(connectionId, out var session);
        return session;
    }

    public IReadOnlyList<ConnectionSession> GetAllConnections()
        => _connections.Values.ToList().AsReadOnly();

    public IReadOnlyList<ConnectionSession> GetConnectionsByIdentity(string identity)
    {
        if (_identityConnections.TryGetValue(identity, out var connectionIds))
            return connectionIds.Select(id => _connections.GetValueOrDefault(id))
                .Where(s => s != null)
                .Select(s => s!)
                .ToList().AsReadOnly();

        return Array.Empty<ConnectionSession>();
    }

    public int ConnectionCount => _connections.Count;

    public bool UpdateHeartbeat(string connectionId)
    {
        if (_connections.TryGetValue(connectionId, out var session))
        {
            session.LastHeartbeat = DateTime.UtcNow;
            return true;
        }
        return false;
    }

    public bool SetState(string connectionId, ConnectionState state)
    {
        if (_connections.TryGetValue(connectionId, out var session))
        {
            session.State = state;
            return true;
        }
        return false;
    }
}
