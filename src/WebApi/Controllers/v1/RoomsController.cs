using Application.Modules.Rooms.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class RoomsController : ControllerBase
{
    private readonly ListRoomsHandler _listHandler;
    private readonly CreateRoomHandler _createHandler;
    private readonly UpdateRoomHandler _updateHandler;
    private readonly DeleteRoomHandler _deleteHandler;

    public RoomsController(
        ListRoomsHandler listHandler,
        CreateRoomHandler createHandler,
        UpdateRoomHandler updateHandler,
        DeleteRoomHandler deleteHandler)
    {
        _listHandler = listHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] bool? activeOnly = true, CancellationToken ct = default)
    {
        var result = await _listHandler.HandleAsync(activeOnly, ct);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request, CancellationToken ct)
    {
        var result = await _createHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomRequest request, CancellationToken ct)
    {
        request.Id = id;
        var result = await _updateHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _deleteHandler.HandleAsync(id, ct);
        if (result.IsSuccess)
            return NoContent();
        return result.ToActionResult();
    }
}
