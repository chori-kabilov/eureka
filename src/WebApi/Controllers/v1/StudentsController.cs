using Application.Modules.Students.Dtos;
using Application.Modules.Students.UseCases.CreateStudent;
using Application.Modules.Students.UseCases.DeleteStudent;
using Application.Modules.Students.UseCases.GetStudent;
using Application.Modules.Students.UseCases.ListStudents;
using Application.Modules.Students.UseCases.UpdateStudent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class StudentsController(
    ListStudentsHandler listHandler,
    GetStudentHandler getHandler,
    CreateStudentHandler createHandler,
    UpdateStudentHandler updateHandler,
    DeleteStudentHandler deleteHandler)
    : ControllerBase
{
    // GET /api/v1/students
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? search,
        [FromQuery] int? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var request = new ListStudentsRequest
        {
            Search = search,
            Status = status,
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

    // GET /api/v1/students/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var result = await getHandler.HandleAsync(id, ct);
        return result.ToActionResult();
    }

    // POST /api/v1/students
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateStudentRequest request,
        CancellationToken ct)
    {
        var result = await createHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // PATCH /api/v1/students/{id}
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateStudentRequest request,
        CancellationToken ct)
    {
        request.Id = id;
        var result = await updateHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // DELETE /api/v1/students/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await deleteHandler.HandleAsync(id, ct);

        if (result.IsSuccess)
            return NoContent();

        return result.ToActionResult();
    }
}
