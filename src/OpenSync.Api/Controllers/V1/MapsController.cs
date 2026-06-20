using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Requests;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Maps.Commands.CreateMap;
using OpenSync.Application.Maps.Commands.DeleteMap;
using OpenSync.Application.Maps.Queries.GetMap;

namespace OpenSync.Api.Controllers.V1;

public class MapsController : BaseController
{
    private readonly IMediator _mediator;

    public MapsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("services/{serviceId:guid}/maps")]
    public async Task<IActionResult> Create(Guid serviceId, [FromBody] CreateMapRequest request)
    {
        var command = new CreateMapCommand(serviceId, request.UniqueName, request.ExpiresAt);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/maps/{id:guid}")]
    public async Task<IActionResult> Get(Guid serviceId, Guid id)
    {
        var result = await _mediator.Send(new GetMapQuery(id));
        if (result.IsSuccess && result.Data != null)
            return Ok(new ApiResponse<MapResponse> { Success = true, Data = MapResponse.FromEntity(result.Data) });
        return ApiResponse(result);
    }

    [HttpDelete("services/{serviceId:guid}/maps/{id:guid}")]
    public async Task<IActionResult> Delete(Guid serviceId, Guid id)
    {
        var result = await _mediator.Send(new DeleteMapCommand(id));
        return ApiResponse(result);
    }
}
