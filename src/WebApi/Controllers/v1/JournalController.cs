using Application.Modules.Journal.UseCases.GetLessonAttendance;
using Application.Modules.Journal.UseCases.MarkAttendance;
using Application.Modules.Journal.UseCases.BulkMarkAttendance;
using Application.Modules.Journal.UseCases.GetLessonGrades;
using Application.Modules.Journal.UseCases.SetGrade;
using Application.Modules.Journal.UseCases.GetGroupJournal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class JournalController(
    GetLessonAttendanceHandler getAttendanceHandler,
    MarkAttendanceHandler markHandler,
    BulkMarkAttendanceHandler bulkMarkHandler,
    GetLessonGradesHandler getGradesHandler,
    SetGradeHandler setGradeHandler,
    GetGroupJournalHandler journalHandler)
    : ControllerBase
{
    // Журнал группы
    [HttpGet("groups/{groupId:guid}")]
    public async Task<IActionResult> GetGroupJournal(
        Guid groupId,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        CancellationToken ct)
    {
        var request = new GetGroupJournalRequest
        {
            GroupId = groupId,
            DateFrom = dateFrom,
            DateTo = dateTo
        };
        var result = await journalHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // Посещаемость занятия
    [HttpGet("lessons/{lessonId:guid}/attendance")]
    public async Task<IActionResult> GetAttendance(Guid lessonId, CancellationToken ct)
    {
        var result = await getAttendanceHandler.HandleAsync(lessonId, ct);
        return result.ToActionResult();
    }

    [HttpPost("lessons/{lessonId:guid}/attendance")]
    public async Task<IActionResult> MarkAttendance(Guid lessonId, [FromBody] MarkAttendanceRequest request, CancellationToken ct)
    {
        request.LessonId = lessonId;
        var result = await markHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpPost("lessons/{lessonId:guid}/attendance/bulk")]
    public async Task<IActionResult> BulkMarkAttendance(Guid lessonId, [FromBody] BulkMarkAttendanceRequest request, CancellationToken ct)
    {
        request.LessonId = lessonId;
        var result = await bulkMarkHandler.HandleAsync(request, ct);
        if (result.IsSuccess)
            return Ok(new { Marked = result.Value });
        return result.ToActionResult();
    }

    // Оценки занятия
    [HttpGet("lessons/{lessonId:guid}/grades")]
    public async Task<IActionResult> GetGrades(Guid lessonId, CancellationToken ct)
    {
        var result = await getGradesHandler.HandleAsync(lessonId, ct);
        return result.ToActionResult();
    }

    [HttpPost("lessons/{lessonId:guid}/grades")]
    public async Task<IActionResult> SetGrade(Guid lessonId, [FromBody] SetGradeRequest request, CancellationToken ct)
    {
        request.LessonId = lessonId;
        var result = await setGradeHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }
}
