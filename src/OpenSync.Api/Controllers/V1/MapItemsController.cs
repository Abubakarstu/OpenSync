using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Requests;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Common.Models;
using OpenSync.Application.Maps.Commands.RemoveMapItem;
using OpenSync.Application.Maps.Commands.SetMapItem;
using OpenSync.Application.Maps.Queries.GetMapItem;
using OpenSync.Application.Maps.Queries.ListMapItems;

namespace OpenSync.Api.Controllers.V1;

public class MapItemsController : BaseController
{
    private readonly IMediator _mediator;

    public MapItemsController(IMediator mediator) => _mediator = mediator;

    [HttpPut("services/{serviceId:guid}/maps/{mapId:guid}/items")]
    public async Task<IActionResult> Set(Guid serviceId, Guid mapId, [FromBody] SetMapItemRequest request)
    {
        var command = new SetMapItemCommand(mapId, request.Key, request.Data, request.ExpiresAt);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/maps/{mapId:guid}/items")]
    public async Task<IActionResult> GetAll(Guid serviceId, Guid mapId, [FromQuery] PageRequest pageRequest)
    {
        var result = await _mediator.Send(new ListMapItemsQuery(mapId, pageRequest));
        return PagedResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/maps/{mapId:guid}/items/{key}")]
    public async Task<IActionResult> Get(Guid serviceId, Guid mapId, string key)
    {
        var result = await _mediator.Send(new GetMapItemQuery(mapId, key));
        if (result.IsSuccess && result.Data != null)
            return Ok(new ApiResponse<MapItemResponse> { Success = true, Data = MapItemResponse.FromEntity(result.Data) });
        return ApiResponse(result);
    }

    [HttpDelete("services/{serviceId:guid}/maps/{mapId:guid}/items/{key}")]
    public async Task<IActionResult> Remove(Guid serviceId, Guid mapId, string key)
    {
        var result = await _mediator.Send(new RemoveMapItemCommand(mapId, key));
        return ApiResponse(result);
    }
}
