using MediatR;

namespace OpenSync.Application.Common.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
