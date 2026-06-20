namespace OpenSync.Api.Extensions;

public static class WebSocketExtensions
{
    public static IApplicationBuilder UseWebSocketEndpoint(this IApplicationBuilder builder, string path = "/ws")
    {
        return builder.UseWhen(context => context.Request.Path == path, app =>
        {
            app.UseWebSockets();
        });
    }
}
