using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Requests;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Lists.Commands.CreateList;
using OpenSync.Application.Lists.Commands.DeleteList;
using OpenSync.Application.Lists.Queries.GetList;

namespace OpenSync.Api.Controllers.V1;

public class ListsController : BaseController
{
    private readonly IMediator _mediator;

    public ListsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("services/{serviceId:guid}/lists")]
    public async Task<IActionResult> Create(Guid serviceId, [FromBody] CreateListRequest request)
    {
        var command = new CreateListCommand(serviceId, request.UniqueName, request.ExpiresAt);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/lists/{id:guid}")]
    public async Task<IActionResult> Get(Guid serviceId, Guid id)
    {
        var result = await _mediator.Send(new GetListQuery(id));
        if (result.IsSuccess && result.Data != null)
            return Ok(new ApiResponse<ListResponse> { Success = true, Data = ListResponse.FromEntity(result.Data) });
        return ApiResponse(result);
    }

    [HttpDelete("services/{serviceId:guid}/lists/{id:guid}")]
    public async Task<IActionResult> Delete(Guid serviceId, Guid id)
    {
        var result = await _mediator.Send(new DeleteListCommand(id));
        return ApiResponse(result);
    }
}
