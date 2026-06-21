using System.Text.Json;
using MediatR;
using OpenSync.Application.Auth.GenerateToken;
using OpenSync.Application.Documents.Commands.CreateDocument;
using OpenSync.AspNetCore.Extensions;
using OpenSync.AspNetCore.Middleware;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenSync(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OpenSyncDbContext>();
    db.Database.EnsureCreated();

    var serviceRepo = scope.ServiceProvider.GetRequiredService<ISyncServiceRepository>();
    var svc = await serviceRepo.GetByNameAsync("demo");
    if (svc == null)
    {
        svc = new SyncService("demo", "Minimal Demo Service");
        await serviceRepo.AddAsync(svc);
        await serviceRepo.SaveChangesAsync();
    }
}

app.UseWebSockets();

app.Map("/ws", b => b.UseMiddleware<SyncWebSocketMiddleware>());

var api = app.MapGroup("/api");

api.MapPost("/tokens", async (IMediator mediator, ITokenService tokenService) =>
{
    var command = new GenerateTokenCommand("demo-user", Guid.Empty, new Dictionary<string, string[]>(), 60);
    var result = await mediator.Send(command);
    return result.IsSuccess
        ? Results.Ok(new { success = true, data = new { token = result.Data } })
        : Results.Json(new { success = false, error = result.ErrorMessage }, statusCode: 401);
});

api.MapPost("/documents/{name}", async (string name, JsonElement body, IMediator mediator) =>
{
    var data = body.TryGetProperty("data", out var d) ? d.GetString() : "{}";
    var command = new CreateDocumentCommand(Guid.Empty, name, data ?? "{}", null, null);
    var result = await mediator.Send(command);
    return Results.Json(new { success = result.IsSuccess, data = result.Data });
});

app.Run("http://localhost:5050");
