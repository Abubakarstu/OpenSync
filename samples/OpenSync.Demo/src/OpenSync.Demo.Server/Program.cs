using System.Text.Json;
using MediatR;
using OpenSync.Application.Auth.GenerateToken;
using OpenSync.Application.Channels.Commands.BroadcastMessage;
using OpenSync.Application.Channels.Commands.JoinChannel;
using OpenSync.Application.Channels.Commands.LeaveChannel;
using OpenSync.Application.Documents.Commands.CreateDocument;
using OpenSync.Application.Streams.Commands.PublishMessage;
using OpenSync.AspNetCore.Extensions;
using OpenSync.AspNetCore.Middleware;
using OpenSync.Core.Entities;
using OpenSync.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenSync(builder.Configuration);

var app = builder.Build();

Guid demoServiceId;
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OpenSyncDbContext>();
    db.Database.EnsureCreated();

    var serviceRepo = scope.ServiceProvider.GetRequiredService<ISyncServiceRepository>();
    var demoService = await serviceRepo.GetByNameAsync("demo");
    if (demoService == null)
    {
        demoService = new SyncService("demo", "OpenSync Demo Service");
        await serviceRepo.AddAsync(demoService);
        await serviceRepo.SaveChangesAsync();
    }
    demoServiceId = demoService.Id;
}

app.UseWebSockets();

app.Map("/ws", builder => builder.UseMiddleware<SyncWebSocketMiddleware>());

var api = app.MapGroup("/api/v1/sync");

api.MapPost("/tokens", async (IMediator mediator, ITokenService tokenService) =>
{
    var command = new GenerateTokenCommand("demo-user", demoServiceId, new Dictionary<string, string[]>(), 60);
    var result = await mediator.Send(command);
    if (result.IsSuccess && result.Data != null)
    {
        var payload = tokenService.ValidateToken(result.Data, "default-secret-key-change-in-production");
        return Results.Ok(new
        {
            success = true,
            data = new
            {
                token = result.Data,
                expiresAt = payload?.ExpiresAt ?? DateTime.UtcNow.AddMinutes(60)
            }
        });
    }
    return Results.Json(new { success = false, errorCode = result.ErrorCode, errorMessage = result.ErrorMessage }, statusCode: 401);
});

api.MapPost("/documents/{uniqueName}", async (string uniqueName, JsonElement body, IMediator mediator) =>
{
    var data = body.TryGetProperty("data", out var d) ? d.GetString() : "{}";
    long? revision = body.TryGetProperty("expected_revision", out var r) && r.ValueKind == JsonValueKind.Number ? r.GetInt64() : null;
    var command = new CreateDocumentCommand(demoServiceId, uniqueName, data ?? "{}", revision, null);
    var result = await mediator.Send(command);
    return Results.Json(new { success = result.IsSuccess, data = result.IsSuccess ? (object?)result.Data : null });
});

api.MapPost("/streams/{uniqueName}/messages", async (string uniqueName, JsonElement body, IMediator mediator, IServiceProvider sp) =>
{
    var streamRepo = sp.GetRequiredService<IStreamRepository>();
    var stream = (await streamRepo.FindAsync(s => s.ServiceId == demoServiceId && s.UniqueName == uniqueName)).FirstOrDefault();
    if (stream == null)
    {
        stream = new OpenSync.Core.Entities.SyncStream(demoServiceId, uniqueName);
        await streamRepo.AddAsync(stream);
        await streamRepo.SaveChangesAsync();
    }
    var data = body.TryGetProperty("data", out var d) ? d.GetString() : "{}";
    var command = new PublishMessageCommand(stream.Id, data ?? "{}", "demo-user");
    await mediator.Send(command);
    return Results.Ok(new { success = true });
});

api.MapPost("/channels/{uniqueName}/members", async (string uniqueName, JsonElement body, IMediator mediator, IServiceProvider sp) =>
{
    var channelRepo = sp.GetRequiredService<IChannelRepository>();
    var channel = (await channelRepo.FindAsync(c => c.ServiceId == demoServiceId && c.UniqueName == uniqueName)).FirstOrDefault();
    if (channel == null)
    {
        channel = new OpenSync.Core.Entities.SyncChannel(demoServiceId, uniqueName);
        await channelRepo.AddAsync(channel);
        await channelRepo.SaveChangesAsync();
    }
    var identity = body.TryGetProperty("identity", out var i) ? i.GetString() : "demo-user";
    var metadata = body.TryGetProperty("metadata", out var m) ? m.GetString() : null;
    var command = new JoinChannelCommand(channel.Id, identity ?? "demo-user", metadata);
    await mediator.Send(command);
    return Results.Ok(new { success = true });
});

api.MapPost("/channels/{uniqueName}/leave", async (string uniqueName, IMediator mediator, IServiceProvider sp) =>
{
    var channelRepo = sp.GetRequiredService<IChannelRepository>();
    var channel = (await channelRepo.FindAsync(c => c.ServiceId == demoServiceId && c.UniqueName == uniqueName)).FirstOrDefault();
    if (channel == null) return Results.Ok(new { success = true });
    var command = new LeaveChannelCommand(channel.Id, "demo-user");
    await mediator.Send(command);
    return Results.Ok(new { success = true });
});

api.MapPost("/channels/{uniqueName}/messages", async (string uniqueName, JsonElement body, IMediator mediator, IServiceProvider sp) =>
{
    var channelRepo = sp.GetRequiredService<IChannelRepository>();
    var channel = (await channelRepo.FindAsync(c => c.ServiceId == demoServiceId && c.UniqueName == uniqueName)).FirstOrDefault();
    if (channel == null) return Results.Json(new { success = false, error = "Channel not found" }, statusCode: 404);
    var data = body.TryGetProperty("data", out var d) ? d.GetString() : "{}";
    var command = new BroadcastMessageCommand(channel.Id, data ?? "{}", "demo-user");
    await mediator.Send(command);
    return Results.Ok(new { success = true });
});

Console.WriteLine("OpenSync Demo Server running on http://localhost:5000");
Console.WriteLine("Press Ctrl+C to stop.");
app.Run("http://0.0.0.0:5000");
