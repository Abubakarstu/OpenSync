using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.SyncServices.Commands.DeleteSyncService;

public record DeleteSyncServiceCommand(Guid Id) : ICommand<Common.Result>;
