using Application.Modules.Groups.Dtos;
using Application.Modules.Groups.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Extensions;
using Domain.Enums;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class GroupsController : ControllerBase
{
    private readonly ListGroupsHandler _listHandler;
    private readonly GetGroupHandler _getHandler;
    private readonly CreateGroupHandler _createHandler;
    private readonly UpdateGroupHandler _updateHandler;
    private readonly DeleteGroupHandler _deleteHandler;
    private readonly ListEnrollmentsHandler _listEnrollmentsHandler;
    private readonly EnrollStudentHandler _enrollHandler;
    private readonly UnenrollStudentHandler _unenrollHandler;
    private readonly TransferStudentHandler _transferHandler;

    public GroupsController(
        ListGroupsHandler listHandler,
        GetGroupHandler getHandler,
        CreateGroupHandler createHandler,
        UpdateGroupHandler updateHandler,
        DeleteGroupHandler deleteHandler,
        ListEnrollmentsHandler listEnrollmentsHandler,
        EnrollStudentHandler enrollHandler,
        UnenrollStudentHandler unenrollHandler,
        TransferStudentHandler transferHandler)
    {
        _listHandler = listHandler;
        _getHandler = getHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _listEnrollmentsHandler = listEnrollmentsHandler;
        _enrollHandler = enrollHandler;
        _unenrollHandler = unenrollHandler;
        _transferHandler = transferHandler;
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? search,
        [FromQuery] Guid? courseId,
        [FromQuery] Guid? teacherId,
        [FromQuery] GroupStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var request = new ListGroupsRequest
        {
            Search = search,
            CourseId = courseId,
            TeacherId = teacherId,
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var result = await _getHandler.HandleAsync(id, ct);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequest request, CancellationToken ct)
    {
        var result = await _createHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGroupRequest request, CancellationToken ct)
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

    // Студенты группы
    [HttpGet("{id:guid}/students")]
    public async Task<IActionResult> ListStudents(Guid id, [FromQuery] EnrollmentStatus? status, CancellationToken ct)
    {
        var result = await _listEnrollmentsHandler.HandleAsync(id, status, ct);
        return result.ToActionResult();
    }

    [HttpPost("{id:guid}/students")]
    public async Task<IActionResult> EnrollStudent(Guid id, [FromBody] EnrollStudentRequest request, CancellationToken ct)
    {
        request.GroupId = id;
        var result = await _enrollHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpDelete("{groupId:guid}/students/{enrollmentId:guid}")]
    public async Task<IActionResult> UnenrollStudent(Guid groupId, Guid enrollmentId, [FromQuery] EnrollmentStatus status = EnrollmentStatus.Expelled, CancellationToken ct = default)
    {
        var result = await _unenrollHandler.HandleAsync(enrollmentId, status, ct);
        if (result.IsSuccess)
            return NoContent();
        return result.ToActionResult();
    }

    [HttpPost("{groupId:guid}/students/{enrollmentId:guid}/transfer")]
    public async Task<IActionResult> TransferStudent(Guid groupId, Guid enrollmentId, [FromBody] TransferStudentRequest request, CancellationToken ct)
    {
        request.EnrollmentId = enrollmentId;
        var result = await _transferHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }
}
