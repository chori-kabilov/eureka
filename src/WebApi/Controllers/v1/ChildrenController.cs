using Application.Modules.Children.Dtos;
using Application.Modules.Children.UseCases.CreateChild;
using Application.Modules.Children.UseCases.DeleteChild;
using Application.Modules.Children.UseCases.GetChild;
using Application.Modules.Children.UseCases.ListChildren;
using Application.Modules.Children.UseCases.UpdateChild;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class ChildrenController : ControllerBase
{
    private readonly ListChildrenHandler _listHandler;
    private readonly GetChildHandler _getHandler;
    private readonly CreateChildHandler _createHandler;
    private readonly UpdateChildHandler _updateHandler;
    private readonly DeleteChildHandler _deleteHandler;

    public ChildrenController(
        ListChildrenHandler listHandler,
        GetChildHandler getHandler,
        CreateChildHandler createHandler,
        UpdateChildHandler updateHandler,
        DeleteChildHandler deleteHandler)
    {
        _listHandler = listHandler;
        _getHandler = getHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

    // GET /api/v1/children
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? search,
        [FromQuery] Guid? parentId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var request = new ListChildrenRequest
        {
            Search = search,
            ParentId = parentId,
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

    // GET /api/v1/children/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var result = await _getHandler.HandleAsync(id, ct);
        return result.ToActionResult();
    }

    // POST /api/v1/children
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateChildRequest request,
        CancellationToken ct)
    {
        var result = await _createHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // PATCH /api/v1/children/{id}
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateChildRequest request,
        CancellationToken ct)
    {
        request.Id = id;
        var result = await _updateHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // DELETE /api/v1/children/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _deleteHandler.HandleAsync(id, ct);

        if (result.IsSuccess)
            return NoContent();

        return result.ToActionResult();
    }
}
