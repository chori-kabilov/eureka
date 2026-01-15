using Application.Modules.Parents.Dtos;
using Application.Modules.Parents.UseCases.CreateParent;
using Application.Modules.Parents.UseCases.DeleteParent;
using Application.Modules.Parents.UseCases.ListParents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class ParentsController : ControllerBase
{
    private readonly ListParentsHandler _listHandler;
    private readonly CreateParentHandler _createHandler;
    private readonly DeleteParentHandler _deleteHandler;

    public ParentsController(
        ListParentsHandler listHandler,
        CreateParentHandler createHandler,
        DeleteParentHandler deleteHandler)
    {
        _listHandler = listHandler;
        _createHandler = createHandler;
        _deleteHandler = deleteHandler;
    }

    // GET /api/v1/parents
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var request = new ListParentsRequest
        {
            Search = search,
            Page = page,
            PageSize = pageSize
        };

        var result = await _listHandler.HandleAsync(request, ct);

        if (result.IsSuccess)
        {
            return Ok(new PagedResponse<object>
            {
                Items = result.Value!.Items.Cast<object>().ToList(),
                Page = result.Value.Page,
                PageSize = result.Value.PageSize,
                TotalCount = result.Value.TotalCount,
                TotalPages = result.Value.TotalPages
            });
        }

        return result.ToActionResult();
    }

    // POST /api/v1/parents
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateParentRequest request,
        CancellationToken ct)
    {
        var result = await _createHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // DELETE /api/v1/parents/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _deleteHandler.HandleAsync(id, ct);

        if (result.IsSuccess)
            return NoContent();

        return result.ToActionResult();
    }
}
