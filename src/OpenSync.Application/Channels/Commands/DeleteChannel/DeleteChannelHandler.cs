using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Channels.Commands.DeleteChannel;

public class DeleteChannelHandler : ICommandHandler<DeleteChannelCommand, Common.Result>
{
    private readonly IChannelRepository _repository;
    private readonly IChannelMemberRepository _memberRepository;

    public DeleteChannelHandler(IChannelRepository repository, IChannelMemberRepository memberRepository)
    {
        _repository = repository;
        _memberRepository = memberRepository;
    }

    public async Task<Common.Result> Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (channel == null)
            return Common.Result.Failure("NOT_FOUND", $"Channel '{request.Id}' not found.");

        await _memberRepository.DeleteByChannelIdAsync(request.Id, cancellationToken);
        await _repository.DeleteAsync(channel, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success();
    }
}
