using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Channels.Commands.CreateChannel;

public class CreateChannelHandler : ICommandHandler<CreateChannelCommand, Common.Result<Guid>>
{
    private readonly IChannelRepository _repository;

    public CreateChannelHandler(IChannelRepository repository) => _repository = repository;

    public async Task<Common.Result<Guid>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.UniqueName))
        {
            var existing = await _repository.GetByServiceAndNameAsync(request.ServiceId, request.UniqueName, cancellationToken);
            if (existing != null)
                return Common.Result.Failure<Guid>("DUPLICATE", $"Channel with name '{request.UniqueName}' already exists.");
        }

        var attributes = new ChannelAttributes
        {
            Type = request.ChannelType,
            IsPrivate = request.IsPrivate ?? false,
            MaxMembers = request.MaxMembers ?? 1000
        };

        var channel = new SyncChannel(request.ServiceId, request.UniqueName, attributes, request.ExpiresAt);
        await _repository.AddAsync(channel, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success(channel.Id);
    }
}
