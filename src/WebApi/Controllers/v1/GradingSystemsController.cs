using Application.Modules.GradingSystems.UseCases.ListGradingSystems;
using Application.Modules.GradingSystems.UseCases.CreateGradingSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/grading-systems")]
[Authorize(Roles = "Admin")]
public class GradingSystemsController(
    ListGradingSystemsHandler listHandler,
    CreateGradingSystemHandler createHandler)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var result = await listHandler.HandleAsync(ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGradingSystemRequest request, CancellationToken ct)
    {
        var result = await createHandler.HandleAsync(request, ct);
        return Ok(new { Success = true, Data = result });
    }
}
