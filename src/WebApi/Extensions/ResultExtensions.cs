using Application.Common;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Common;

namespace WebApi.Extensions;

// Extension для конвертации Result в IActionResult
public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(ApiResponse<T>.Ok(result.Value!));
        }

        return result.Error.Code switch
        {
            "NOT_FOUND" => new NotFoundObjectResult(ApiResponse<T>.Fail(result.Error.Code, result.Error.Message)),
            "VALIDATION_ERROR" => new BadRequestObjectResult(ApiResponse<T>.Fail(result.Error.Code, result.Error.Message)),
            _ => new BadRequestObjectResult(ApiResponse<T>.Fail(result.Error.Code, result.Error.Message))
        };
    }

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(ApiResponse<object>.Ok(new { }));
        }

        return result.Error.Code switch
        {
            "NOT_FOUND" => new NotFoundObjectResult(ApiResponse<object>.Fail(result.Error.Code, result.Error.Message)),
            _ => new BadRequestObjectResult(ApiResponse<object>.Fail(result.Error.Code, result.Error.Message))
        };
    }
}
