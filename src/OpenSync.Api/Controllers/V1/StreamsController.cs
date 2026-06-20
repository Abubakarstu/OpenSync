using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Requests;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Streams.Commands.CreateStream;
using OpenSync.Application.Streams.Commands.DeleteStream;
using OpenSync.Application.Streams.Commands.PublishMessage;
using OpenSync.Application.Streams.Queries.GetStream;

namespace OpenSync.Api.Controllers.V1;

public class StreamsController : BaseController
{
    private readonly IMediator _mediator;

    public StreamsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("services/{serviceId:guid}/streams")]
    public async Task<IActionResult> Create(Guid serviceId, [FromBody] CreateStreamRequest request)
    {
        var command = new CreateStreamCommand(serviceId, request.UniqueName, request.ExpiresAt);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/streams/{id:guid}")]
    public async Task<IActionResult> Get(Guid serviceId, Guid id)
    {
        var result = await _mediator.Send(new GetStreamQuery(id));
        if (result.IsSuccess && result.Data != null)
            return Ok(new ApiResponse<StreamResponse> { Success = true, Data = StreamResponse.FromEntity(result.Data) });
        return ApiResponse(result);
    }

    [HttpPost("services/{serviceId:guid}/streams/{id:guid}/messages")]
    public async Task<IActionResult> PublishMessage(Guid serviceId, Guid id, [FromBody] PublishMessageRequest request)
    {
        var command = new PublishMessageCommand(id, request.Data, null);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpDelete("services/{serviceId:guid}/streams/{id:guid}")]
    public async Task<IActionResult> Delete(Guid serviceId, Guid id)
    {
        var result = await _mediator.Send(new DeleteStreamCommand(id));
        return ApiResponse(result);
    }
}
