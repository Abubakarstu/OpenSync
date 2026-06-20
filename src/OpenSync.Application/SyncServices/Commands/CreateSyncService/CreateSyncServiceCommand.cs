using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.SyncServices.Commands.CreateSyncService;

public record CreateSyncServiceCommand(
    string Name,
    string? Description,
    string? WebhookUrl,
    string? WebhookSecret
) : ICommand<Common.Result<Guid>>;
