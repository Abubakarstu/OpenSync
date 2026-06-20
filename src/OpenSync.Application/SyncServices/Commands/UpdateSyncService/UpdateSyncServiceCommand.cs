using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.SyncServices.Commands.UpdateSyncService;

public record UpdateSyncServiceCommand(
    Guid Id,
    string Name,
    string? Description,
    string? WebhookUrl,
    string? WebhookSecret
) : ICommand<Common.Result>;
