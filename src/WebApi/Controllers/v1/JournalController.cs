using Application.Modules.Journal.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class JournalController : ControllerBase
{
    private readonly GetLessonAttendanceHandler _getAttendanceHandler;
    private readonly MarkAttendanceHandler _markHandler;
    private readonly BulkMarkAttendanceHandler _bulkMarkHandler;
    private readonly GetLessonGradesHandler _getGradesHandler;
    private readonly SetGradeHandler _setGradeHandler;
    private readonly GetGroupJournalHandler _journalHandler;

    public JournalController(
        GetLessonAttendanceHandler getAttendanceHandler,
        MarkAttendanceHandler markHandler,
        BulkMarkAttendanceHandler bulkMarkHandler,
        GetLessonGradesHandler getGradesHandler,
        SetGradeHandler setGradeHandler,
        GetGroupJournalHandler journalHandler)
    {
        _getAttendanceHandler = getAttendanceHandler;
        _markHandler = markHandler;
        _bulkMarkHandler = bulkMarkHandler;
        _getGradesHandler = getGradesHandler;
        _setGradeHandler = setGradeHandler;
        _journalHandler = journalHandler;
    }

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
        var result = await _journalHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    // Посещаемость занятия
    [HttpGet("lessons/{lessonId:guid}/attendance")]
    public async Task<IActionResult> GetAttendance(Guid lessonId, CancellationToken ct)
    {
        var result = await _getAttendanceHandler.HandleAsync(lessonId, ct);
        return result.ToActionResult();
    }

    [HttpPost("lessons/{lessonId:guid}/attendance")]
    public async Task<IActionResult> MarkAttendance(Guid lessonId, [FromBody] MarkAttendanceRequest request, CancellationToken ct)
    {
        request.LessonId = lessonId;
        var result = await _markHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpPost("lessons/{lessonId:guid}/attendance/bulk")]
    public async Task<IActionResult> BulkMarkAttendance(Guid lessonId, [FromBody] BulkMarkAttendanceRequest request, CancellationToken ct)
    {
        request.LessonId = lessonId;
        var result = await _bulkMarkHandler.HandleAsync(request, ct);
        if (result.IsSuccess)
            return Ok(new { Marked = result.Value });
        return result.ToActionResult();
    }

    // Оценки занятия
    [HttpGet("lessons/{lessonId:guid}/grades")]
    public async Task<IActionResult> GetGrades(Guid lessonId, CancellationToken ct)
    {
        var result = await _getGradesHandler.HandleAsync(lessonId, ct);
        return result.ToActionResult();
    }

    [HttpPost("lessons/{lessonId:guid}/grades")]
    public async Task<IActionResult> SetGrade(Guid lessonId, [FromBody] SetGradeRequest request, CancellationToken ct)
    {
        request.LessonId = lessonId;
        var result = await _setGradeHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }
}
