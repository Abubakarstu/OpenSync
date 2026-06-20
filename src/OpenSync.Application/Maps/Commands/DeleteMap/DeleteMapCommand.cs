using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Maps.Commands.DeleteMap;

public record DeleteMapCommand(Guid Id) : ICommand<Common.Result>;
