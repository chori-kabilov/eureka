using Application.Modules.Rooms.UseCases.ListRooms;
using Application.Modules.Rooms.UseCases.CreateRoom;
using Application.Modules.Rooms.UseCases.UpdateRoom;
using Application.Modules.Rooms.UseCases.DeleteRoom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class RoomsController(
    ListRoomsHandler listHandler,
    CreateRoomHandler createHandler,
    UpdateRoomHandler updateHandler,
    DeleteRoomHandler deleteHandler)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] bool? activeOnly = true, CancellationToken ct = default)
    {
        var result = await listHandler.HandleAsync(activeOnly, ct);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request, CancellationToken ct)
    {
        var result = await createHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomRequest request, CancellationToken ct)
    {
        request.Id = id;
        var result = await updateHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await deleteHandler.HandleAsync(id, ct);
        if (result.IsSuccess)
            return NoContent();
        return result.ToActionResult();
    }
}
