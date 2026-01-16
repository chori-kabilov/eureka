using Application.Modules.Schedule.UseCases;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class LessonsController : ControllerBase
{
    private readonly ListLessonsHandler _listHandler;
    private readonly CreateLessonHandler _createHandler;
    private readonly GenerateLessonsHandler _generateHandler;
    private readonly CancelLessonHandler _cancelHandler;

    public LessonsController(
        ListLessonsHandler listHandler,
        CreateLessonHandler createHandler,
        GenerateLessonsHandler generateHandler,
        CancelLessonHandler cancelHandler)
    {
        _listHandler = listHandler;
        _createHandler = createHandler;
        _generateHandler = generateHandler;
        _cancelHandler = cancelHandler;
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] Guid? groupId,
        [FromQuery] Guid? teacherId,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        [FromQuery] LessonStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken ct = default)
    {
        var request = new ListLessonsRequest
        {
            GroupId = groupId,
            TeacherId = teacherId,
            DateFrom = dateFrom,
            DateTo = dateTo,
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLessonRequest request, CancellationToken ct)
    {
        var result = await _createHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateLessonsRequest request, CancellationToken ct)
    {
        var result = await _generateHandler.HandleAsync(request, ct);
        if (result.IsSuccess)
            return Ok(new { Generated = result.Value });
        return result.ToActionResult();
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelRequest? request, CancellationToken ct)
    {
        var result = await _cancelHandler.HandleAsync(id, request?.Reason, ct);
        if (result.IsSuccess)
            return NoContent();
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}/attendance-form")]
    public async Task<IActionResult> GetAttendanceForm(Guid id, [FromServices] GetAttendanceFormHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(id, ct);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}/grades-form")]
    public async Task<IActionResult> GetGradesForm(Guid id, [FromServices] GetGradesFormHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(id, ct);
        return result.ToActionResult();
    }
}

public class CancelRequest
{
    public string? Reason { get; set; }
}
