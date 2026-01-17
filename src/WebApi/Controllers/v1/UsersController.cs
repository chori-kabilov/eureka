using Application.Common;
using Application.Modules.Users.UseCases.DeleteUser;
using Application.Modules.Users.UseCases.GetUser;
using Application.Modules.Users.UseCases.ListUsers;
using Application.Modules.Users.UseCases.UpdateUserRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController(
    ListUsersHandler listHandler,
    GetUserHandler getHandler,
    UpdateUserRoleHandler updateRoleHandler,
    DeleteUserHandler deleteHandler)
    : ControllerBase
{
    // GET /api/v1/users
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? search,
        [FromQuery] bool? isAdmin,
        [FromQuery] bool? hasStudent,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var request = new ListUsersRequest
        {
            Search = search,
            IsAdmin = isAdmin,
            HasStudent = hasStudent,
            Page = page,
            PageSize = pageSize
        };

        var result = await listHandler.HandleAsync(request, ct);

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

    // GET /api/v1/users/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var result = await getHandler.HandleAsync(id, ct);
        return result.ToActionResult();
    }

    // PATCH /api/v1/users/{id}/admin
    [HttpPatch("{id:guid}/admin")]
    public async Task<IActionResult> UpdateAdmin(
        Guid id,
        [FromBody] UpdateUserRoleRequest request,
        CancellationToken ct)
    {
        request.UserId = id;
        var result = await updateRoleHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // DELETE /api/v1/users/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await deleteHandler.HandleAsync(id, ct);

        if (result.IsSuccess)
            return NoContent();

        return result.ToActionResult();
    }
}
