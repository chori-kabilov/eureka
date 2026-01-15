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
public class StudentsController : ControllerBase
{
    private readonly ListStudentsHandler _listHandler;
    private readonly GetStudentHandler _getHandler;
    private readonly CreateStudentHandler _createHandler;
    private readonly UpdateStudentHandler _updateHandler;
    private readonly DeleteStudentHandler _deleteHandler;

    public StudentsController(
        ListStudentsHandler listHandler,
        GetStudentHandler getHandler,
        CreateStudentHandler createHandler,
        UpdateStudentHandler updateHandler,
        DeleteStudentHandler deleteHandler)
    {
        _listHandler = listHandler;
        _getHandler = getHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

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

    // GET /api/v1/students/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var result = await _getHandler.HandleAsync(id, ct);
        return result.ToActionResult();
    }

    // POST /api/v1/students
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateStudentRequest request,
        CancellationToken ct)
    {
        var result = await _createHandler.HandleAsync(request, ct);
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
        var result = await _updateHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // DELETE /api/v1/students/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _deleteHandler.HandleAsync(id, ct);

        if (result.IsSuccess)
            return NoContent();

        return result.ToActionResult();
    }
}
