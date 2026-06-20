using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Requests;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Common.Models;
using OpenSync.Application.Lists.Commands.AddListItem;
using OpenSync.Application.Lists.Commands.InsertListItem;
using OpenSync.Application.Lists.Commands.RemoveListItem;
using OpenSync.Application.Lists.Commands.UpdateListItem;
using OpenSync.Application.Lists.Queries.GetListItem;
using OpenSync.Application.Lists.Queries.ListItems;

namespace OpenSync.Api.Controllers.V1;

public class ListItemsController : BaseController
{
    private readonly IMediator _mediator;

    public ListItemsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("services/{serviceId:guid}/lists/{listId:guid}/items")]
    public async Task<IActionResult> Add(Guid serviceId, Guid listId, [FromBody] AddListItemRequest request)
    {
        var command = new AddListItemCommand(listId, request.Data, request.ExpiresAt);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpPost("services/{serviceId:guid}/lists/{listId:guid}/items/insert")]
    public async Task<IActionResult> Insert(Guid serviceId, Guid listId, [FromQuery] int index, [FromBody] AddListItemRequest request)
    {
        var command = new InsertListItemCommand(listId, index, request.Data, request.ExpiresAt);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/lists/{listId:guid}/items")]
    public async Task<IActionResult> GetAll(Guid serviceId, Guid listId, [FromQuery] PageRequest pageRequest)
    {
        var result = await _mediator.Send(new ListItemsQuery(listId, pageRequest));
        return PagedResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/lists/{listId:guid}/items/{id:guid}")]
    public async Task<IActionResult> Get(Guid serviceId, Guid listId, Guid id)
    {
        var result = await _mediator.Send(new GetListItemQuery(id));
        if (result.IsSuccess && result.Data != null)
            return Ok(new ApiResponse<ListItemResponse> { Success = true, Data = ListItemResponse.FromEntity(result.Data) });
        return ApiResponse(result);
    }

    [HttpPut("services/{serviceId:guid}/lists/{listId:guid}/items/{id:guid}")]
    public async Task<IActionResult> Update(Guid serviceId, Guid listId, Guid id, [FromBody] UpdateDocumentRequest request)
    {
        var command = new UpdateListItemCommand(id, request.Data, request.ExpectedRevision);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpDelete("services/{serviceId:guid}/lists/{listId:guid}/items")]
    public async Task<IActionResult> Remove(Guid serviceId, Guid listId, [FromQuery] int index)
    {
        var command = new RemoveListItemCommand(listId, index);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }
}
