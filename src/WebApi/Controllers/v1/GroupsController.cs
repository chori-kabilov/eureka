using Application.Modules.Groups.Dtos;
using Application.Modules.Groups.UseCases.ListGroups;
using Application.Modules.Groups.UseCases.GetGroup;
using Application.Modules.Groups.UseCases.CreateGroup;
using Application.Modules.Groups.UseCases.UpdateGroup;
using Application.Modules.Groups.UseCases.DeleteGroup;
using Application.Modules.Groups.UseCases.ListEnrollments;
using Application.Modules.Groups.UseCases.EnrollStudent;
using Application.Modules.Groups.UseCases.UnenrollStudent;
using Application.Modules.Groups.UseCases.TransferStudent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Extensions;
using Domain.Groups;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class GroupsController(
    ListGroupsHandler listHandler,
    GetGroupHandler getHandler,
    CreateGroupHandler createHandler,
    UpdateGroupHandler updateHandler,
    DeleteGroupHandler deleteHandler,
    ListEnrollmentsHandler listEnrollmentsHandler,
    EnrollStudentHandler enrollHandler,
    UnenrollStudentHandler unenrollHandler,
    TransferStudentHandler transferHandler)
    : ControllerBase
{
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var result = await getHandler.HandleAsync(id, ct);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequest request, CancellationToken ct)
    {
        var result = await createHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGroupRequest request, CancellationToken ct)
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

    // Студенты группы
    [HttpGet("{id:guid}/students")]
    public async Task<IActionResult> ListStudents(Guid id, [FromQuery] EnrollmentStatus? status, CancellationToken ct)
    {
        var result = await listEnrollmentsHandler.HandleAsync(id, status, ct);
        return result.ToActionResult();
    }

    [HttpPost("{id:guid}/students")]
    public async Task<IActionResult> EnrollStudent(Guid id, [FromBody] EnrollStudentRequest request, CancellationToken ct)
    {
        request.GroupId = id;
        var result = await enrollHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpDelete("{groupId:guid}/students/{enrollmentId:guid}")]
    public async Task<IActionResult> UnenrollStudent(Guid groupId, Guid enrollmentId, [FromQuery] EnrollmentStatus status = EnrollmentStatus.Expelled, CancellationToken ct = default)
    {
        var result = await unenrollHandler.HandleAsync(enrollmentId, status, ct);
        if (result.IsSuccess)
            return NoContent();
        return result.ToActionResult();
    }

    [HttpPost("{groupId:guid}/students/{enrollmentId:guid}/transfer")]
    public async Task<IActionResult> TransferStudent(Guid groupId, Guid enrollmentId, [FromBody] TransferStudentRequest request, CancellationToken ct)
    {
        request.EnrollmentId = enrollmentId;
        var result = await transferHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }
}
