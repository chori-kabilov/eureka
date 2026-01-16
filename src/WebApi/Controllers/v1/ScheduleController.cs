using Application.Modules.Schedule.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/schedule")]
[Authorize(Roles = "Admin")]
public class ScheduleController : ControllerBase
{
    private readonly ListScheduleTemplatesHandler _listHandler;
    private readonly CreateScheduleTemplateHandler _createHandler;
    private readonly DeleteScheduleTemplateHandler _deleteHandler;

    public ScheduleController(
        ListScheduleTemplatesHandler listHandler,
        CreateScheduleTemplateHandler createHandler,
        DeleteScheduleTemplateHandler deleteHandler)
    {
        _listHandler = listHandler;
        _createHandler = createHandler;
        _deleteHandler = deleteHandler;
    }

    [HttpGet("groups/{groupId:guid}/templates")]
    public async Task<IActionResult> ListTemplates(Guid groupId, CancellationToken ct)
    {
        var result = await _listHandler.HandleAsync(groupId, ct);
        return result.ToActionResult();
    }

    [HttpPost("groups/{groupId:guid}/templates")]
    public async Task<IActionResult> CreateTemplate(Guid groupId, [FromBody] CreateScheduleTemplateRequest request, CancellationToken ct)
    {
        request.GroupId = groupId;
        var result = await _createHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpDelete("templates/{id:guid}")]
    public async Task<IActionResult> DeleteTemplate(Guid id, CancellationToken ct)
    {
        var result = await _deleteHandler.HandleAsync(id, ct);
        if (result.IsSuccess)
            return NoContent();
        return result.ToActionResult();
    }
}
