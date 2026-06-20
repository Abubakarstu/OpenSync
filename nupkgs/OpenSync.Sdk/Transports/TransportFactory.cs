namespace OpenSync.Sdk.Transports;

public static class TransportFactory
{
    public static ITransport Create(TransportType type, string baseUrl, object serializer, object tokenProvider)
    {
        return type switch
        {
            TransportType.WebSocket => new WebSocketTransport(baseUrl),
            TransportType.Sse => new SseTransport(baseUrl),
            _ => new WebSocketTransport(baseUrl)
        };
    }
}
