using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Lists.Commands.DeleteList;

public record DeleteListCommand(Guid Id) : ICommand<Common.Result>;
