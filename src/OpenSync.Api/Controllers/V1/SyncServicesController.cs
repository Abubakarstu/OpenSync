using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Common.Models;
using OpenSync.Application.SyncServices.Commands.CreateSyncService;
using OpenSync.Application.SyncServices.Commands.DeleteSyncService;
using OpenSync.Application.SyncServices.Commands.UpdateSyncService;
using OpenSync.Application.SyncServices.Queries.GetSyncService;
using OpenSync.Application.SyncServices.Queries.ListSyncServices;

namespace OpenSync.Api.Controllers.V1;

public class SyncServicesController : BaseController
{
    private readonly IMediator _mediator;

    public SyncServicesController(IMediator mediator) => _mediator = mediator;

    [HttpPost("services")]
    public async Task<IActionResult> Create([FromBody] CreateSyncServiceCommand command)
    {
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpGet("services")]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
    {
        var result = await _mediator.Send(new ListSyncServicesQuery(pageRequest));
        return PagedResponse(result);
    }

    [HttpGet("services/{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _mediator.Send(new GetSyncServiceQuery(id));
        if (result.IsSuccess && result.Data != null)
            return Ok(new ApiResponse<SyncServiceResponse> { Success = true, Data = SyncServiceResponse.FromEntity(result.Data) });
        return ApiResponse(result);
    }

    [HttpPut("services/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSyncServiceCommand command)
    {
        if (id != command.Id)
            return BadRequest(new ErrorResponse { Success = false, Error = new ErrorDetail { Code = "ID_MISMATCH", Message = "Route ID and body ID must match." } });
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpDelete("services/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteSyncServiceCommand(id));
        return ApiResponse(result);
    }
}
