using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Channels.Queries.ListChannelMembers;
using OpenSync.Application.Common.Models;

namespace OpenSync.Api.Controllers.V1;

public class ChannelMembersController : BaseController
{
    private readonly IMediator _mediator;

    public ChannelMembersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("services/{serviceId:guid}/channels/{channelId:guid}/members")]
    public async Task<IActionResult> GetAll(Guid serviceId, Guid channelId, [FromQuery] PageRequest pageRequest)
    {
        var result = await _mediator.Send(new ListChannelMembersQuery(channelId, pageRequest));
        return PagedResponse(result);
    }
}
