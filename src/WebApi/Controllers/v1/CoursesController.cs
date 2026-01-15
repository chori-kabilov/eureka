using Application.Common;
using Application.Modules.Courses.Dtos;
using Application.Modules.Courses.UseCases.ArchiveCourse;
using Application.Modules.Courses.UseCases.CreateCourse;
using Application.Modules.Courses.UseCases.DeleteCourse;
using Application.Modules.Courses.UseCases.GetCourse;
using Application.Modules.Courses.UseCases.ListCourses;
using Application.Modules.Courses.UseCases.UpdateCourse;
using Domain.Courses;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Courses;
using WebApi.Extensions;
using WebApi.Contracts.Common;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class CoursesController(
    CreateCourseHandler createHandler,
    UpdateCourseHandler updateHandler,
    GetCourseHandler getHandler,
    ListCoursesHandler listHandler,
    ArchiveCourseHandler archiveHandler,
    DeleteCourseHandler deleteHandler)
    : ControllerBase
{
    // GET /api/v1/courses
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] CourseStatus? status = null,
        [FromQuery] StudentPaymentType? studentPaymentType = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = false,
        CancellationToken ct = default)
    {
        var request = new ListCoursesRequest
        {
            Pagination = new PaginationParams { Page = page, PageSize = pageSize },
            Filter = new CourseFilterDto
            {
                Search = search,
                Status = status,
                StudentPaymentType = studentPaymentType,
                SortBy = sortBy,
                SortDesc = sortDesc
            }
        };

        var result = await listHandler.HandleAsync(request, ct);

        if (result.IsFailure)
            return result.ToActionResult();

        return Ok(new PagedResponse<CourseDto>
        {
            Items = result.Value!.Items,
            Page = result.Value.Page,
            PageSize = result.Value.PageSize,
            TotalCount = result.Value.TotalCount,
            TotalPages = result.Value.TotalPages
        });
    }

    // GET /api/v1/courses/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await getHandler.HandleAsync(id, ct);
        return result.ToActionResult();
    }

    // POST /api/v1/courses
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCourseApiRequest apiRequest,
        CancellationToken ct)
    {
        var request = new CreateCourseRequest
        {
            Name = apiRequest.Name,
            Description = apiRequest.Description,
            StudentPaymentType = apiRequest.StudentPaymentType,
            AbsencePolicy = apiRequest.AbsencePolicy,
            TeacherPaymentType = apiRequest.TeacherPaymentType
        };

        var result = await createHandler.HandleAsync(request, ct);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, 
                ApiResponse<CourseDetailDto>.Ok(result.Value));

        return result.ToActionResult();
    }

    // PUT /api/v1/courses/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCourseApiRequest apiRequest,
        CancellationToken ct)
    {
        var request = new UpdateCourseRequest
        {
            Id = id,
            Name = apiRequest.Name,
            Description = apiRequest.Description,
            StudentPaymentType = apiRequest.StudentPaymentType,
            AbsencePolicy = apiRequest.AbsencePolicy,
            TeacherPaymentType = apiRequest.TeacherPaymentType
        };

        var result = await updateHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // PATCH /api/v1/courses/{id}/archive
    [HttpPatch("{id:guid}/archive")]
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct)
    {
        var result = await archiveHandler.HandleAsync(id, ct);
        return result.ToActionResult();
    }

    // DELETE /api/v1/courses/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await deleteHandler.HandleAsync(id, ct);
        return result.ToActionResult();
    }
}
