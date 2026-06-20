using OpenSync.Application;
using OpenSync.Infrastructure;
using OpenSync.Infrastructure.Auth;
using OpenSync.Infrastructure.Middleware;
using OpenSync.Api.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/opensync-.log", rollingInterval: RollingInterval.Day));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddOpenSyncApplication();
    builder.Services.AddOpenSyncInfrastructure(builder.Configuration);
    builder.Services.AddOpenSyncApiServices();

    builder.Services.AddAuthentication()
        .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, null);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseCorrelationId();
    app.UseGlobalException();
    app.UseAuthentication();
    app.UseTokenValidation();
    app.UseApiKey();
    app.UseWebSockets();
    app.MapControllers();

    app.Map("/ws", async context =>
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var handler = context.RequestServices.GetRequiredService<OpenSync.Api.WebSockets.SyncWebSocketEndpoint>();
            await handler.HandleAsync(context);
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Expected a WebSocket request");
        }
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
