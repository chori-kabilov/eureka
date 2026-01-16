using Application.Modules.Schedule.UseCases.ListScheduleTemplates;
using Application.Modules.Schedule.UseCases.CreateScheduleTemplate;
using Application.Modules.Schedule.UseCases.DeleteScheduleTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/schedule")]
[Authorize(Roles = "Admin")]
public class ScheduleController(
    ListScheduleTemplatesHandler listHandler,
    CreateScheduleTemplateHandler createHandler,
    DeleteScheduleTemplateHandler deleteHandler)
    : ControllerBase
{
    [HttpGet("groups/{groupId:guid}/templates")]
    public async Task<IActionResult> ListTemplates(Guid groupId, CancellationToken ct)
    {
        var result = await listHandler.HandleAsync(groupId, ct);
        return result.ToActionResult();
    }

    [HttpPost("groups/{groupId:guid}/templates")]
    public async Task<IActionResult> CreateTemplate(Guid groupId, [FromBody] CreateScheduleTemplateRequest request, CancellationToken ct)
    {
        request.GroupId = groupId;
        var result = await createHandler.HandleAsync(request, ct);
        return result.ToActionResult();
    }

    [HttpDelete("templates/{id:guid}")]
    public async Task<IActionResult> DeleteTemplate(Guid id, CancellationToken ct)
    {
        var result = await deleteHandler.HandleAsync(id, ct);
        if (result.IsSuccess)
            return NoContent();
        return result.ToActionResult();
    }
}
