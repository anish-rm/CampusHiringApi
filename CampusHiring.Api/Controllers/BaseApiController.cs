using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace CampusHiring.Api.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<T> ToActionResult<T>(Result<T> result)
        => result.IsSuccess
            ? Ok(result.Value)
            : MapToErrors(result.Errors);

    protected ActionResult ToActionResult(Result result)
        => result.IsSuccess
            ? NoContent()
            : MapToErrors(result.Errors);

    protected ActionResult MapToErrors(Error[] errors)
    {
        if(errors is null || errors.Length == 0)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An error occured",
                detail: "No error details provided"
                );
        }

        var e = errors[0];

        var errorDetails = string.Join("; ", errors.Select(e => e.Description));

        return e.Code switch
        {
            ErrorCodes.NotFound => Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Resource not found",
                detail: errorDetails
                ),
            ErrorCodes.Validation => ValidationProblem(
                title: "Validation Failed",
                detail: errorDetails
                ),
            ErrorCodes.BadRequest => Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: errorDetails
                ),
            ErrorCodes.Forbid => Problem(
                statusCode: StatusCodes.Status403Forbidden,
                title: "Forbidden",
                detail: errorDetails
                ),
            _ => Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: errorDetails,
                title: e.Code
                ),
        };
    }
}
