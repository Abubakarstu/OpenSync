using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Requests;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Channels.Commands.BroadcastMessage;
using OpenSync.Application.Channels.Commands.CreateChannel;
using OpenSync.Application.Channels.Commands.DeleteChannel;
using OpenSync.Application.Channels.Commands.JoinChannel;
using OpenSync.Application.Channels.Commands.LeaveChannel;
using OpenSync.Application.Channels.Commands.UpdatePresence;
using OpenSync.Application.Channels.Queries.GetChannel;
using OpenSync.Application.Channels.Queries.ListChannels;
using OpenSync.Application.Common.Models;

namespace OpenSync.Api.Controllers.V1;

public class ChannelsController : BaseController
{
    private readonly IMediator _mediator;

    public ChannelsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("services/{serviceId:guid}/channels")]
    public async Task<IActionResult> Create(Guid serviceId, [FromBody] CreateChannelRequest request)
    {
        var command = new CreateChannelCommand(serviceId, request.UniqueName, request.ChannelType, request.IsPrivate, request.MaxMembers, request.ExpiresAt);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/channels")]
    public async Task<IActionResult> GetAll(Guid serviceId, [FromQuery] PageRequest pageRequest)
    {
        var result = await _mediator.Send(new ListChannelsQuery(serviceId, pageRequest));
        return PagedResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/channels/{id:guid}")]
    public async Task<IActionResult> Get(Guid serviceId, Guid id)
    {
        var result = await _mediator.Send(new GetChannelQuery(id));
        if (result.IsSuccess && result.Data != null)
            return Ok(new ApiResponse<ChannelResponse> { Success = true, Data = ChannelResponse.FromEntity(result.Data) });
        return ApiResponse(result);
    }

    [HttpPost("services/{serviceId:guid}/channels/{id:guid}/join")]
    public async Task<IActionResult> Join(Guid serviceId, Guid id, [FromBody] JoinChannelRequest request)
    {
        var result = await _mediator.Send(new JoinChannelCommand(id, request.Identity, request.Metadata));
        return ApiResponse(result);
    }

    [HttpPost("services/{serviceId:guid}/channels/{id:guid}/leave")]
    public async Task<IActionResult> Leave(Guid serviceId, Guid id, [FromBody] JoinChannelRequest request)
    {
        var result = await _mediator.Send(new LeaveChannelCommand(id, request.Identity));
        return ApiResponse(result);
    }

    [HttpPut("services/{serviceId:guid}/channels/{id:guid}/presence")]
    public async Task<IActionResult> UpdatePresence(Guid serviceId, Guid id, [FromBody] UpdatePresenceRequest request)
    {
        var identity = User.FindFirst("identity")?.Value ?? "unknown";
        var result = await _mediator.Send(new UpdatePresenceCommand(id, identity, request.Metadata));
        return ApiResponse(result);
    }

    [HttpPost("services/{serviceId:guid}/channels/{id:guid}/broadcast")]
    public async Task<IActionResult> Broadcast(Guid serviceId, Guid id, [FromBody] BroadcastMessageRequest request)
    {
        var publisherId = User.FindFirst("identity")?.Value;
        var result = await _mediator.Send(new BroadcastMessageCommand(id, request.Data, publisherId));
        return ApiResponse(result);
    }

    [HttpDelete("services/{serviceId:guid}/channels/{id:guid}")]
    public async Task<IActionResult> Delete(Guid serviceId, Guid id)
    {
        var result = await _mediator.Send(new DeleteChannelCommand(id));
        return ApiResponse(result);
    }
}
