using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Requests;
using OpenSync.Api.Models.Responses;
using OpenSync.Application.Common.Models;
using OpenSync.Application.Documents.Commands.CreateDocument;
using OpenSync.Application.Documents.Commands.DeleteDocument;
using OpenSync.Application.Documents.Commands.PatchDocument;
using OpenSync.Application.Documents.Commands.UpdateDocument;
using OpenSync.Application.Documents.Queries.GetDocument;
using OpenSync.Application.Documents.Queries.GetDocumentByName;
using OpenSync.Application.Documents.Queries.ListDocuments;

namespace OpenSync.Api.Controllers.V1;

public class DocumentsController : BaseController
{
    private readonly IMediator _mediator;

    public DocumentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("services/{serviceId:guid}/documents")]
    public async Task<IActionResult> Create(Guid serviceId, [FromBody] CreateDocumentRequest request)
    {
        var command = new CreateDocumentCommand(serviceId, request.UniqueName, request.Data, request.ExpectedRevision, request.ExpiresAt);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/documents")]
    public async Task<IActionResult> GetAll(Guid serviceId, [FromQuery] PageRequest pageRequest)
    {
        var result = await _mediator.Send(new ListDocumentsQuery(serviceId, pageRequest));
        return PagedResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/documents/{id:guid}")]
    public async Task<IActionResult> Get(Guid serviceId, Guid id)
    {
        var result = await _mediator.Send(new GetDocumentQuery(id));
        if (result.IsSuccess && result.Data != null)
            return Ok(new ApiResponse<DocumentResponse> { Success = true, Data = DocumentResponse.FromEntity(result.Data) });
        return ApiResponse(result);
    }

    [HttpGet("services/{serviceId:guid}/documents/by-name/{uniqueName}")]
    public async Task<IActionResult> GetByName(Guid serviceId, string uniqueName)
    {
        var result = await _mediator.Send(new GetDocumentByNameQuery(serviceId, uniqueName));
        if (result.IsSuccess && result.Data != null)
            return Ok(new ApiResponse<DocumentResponse> { Success = true, Data = DocumentResponse.FromEntity(result.Data) });
        return ApiResponse(result);
    }

    [HttpPut("services/{serviceId:guid}/documents/{id:guid}")]
    public async Task<IActionResult> Update(Guid serviceId, Guid id, [FromBody] UpdateDocumentRequest request)
    {
        var command = new UpdateDocumentCommand(id, request.Data, request.ExpectedRevision);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpPatch("services/{serviceId:guid}/documents/{id:guid}")]
    public async Task<IActionResult> Patch(Guid serviceId, Guid id, [FromBody] PatchDocumentRequest request)
    {
        var command = new PatchDocumentCommand(id, request.Data, request.ExpectedRevision);
        var result = await _mediator.Send(command);
        return ApiResponse(result);
    }

    [HttpDelete("services/{serviceId:guid}/documents/{id:guid}")]
    public async Task<IActionResult> Delete(Guid serviceId, Guid id)
    {
        var result = await _mediator.Send(new DeleteDocumentCommand(id));
        return ApiResponse(result);
    }
}
