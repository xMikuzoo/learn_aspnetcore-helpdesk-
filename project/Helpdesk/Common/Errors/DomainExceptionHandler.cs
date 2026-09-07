using Microsoft.AspNetCore.Diagnostics;

namespace Helpdesk.Common.Errors;

public class DomainExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DomainException domainException)
        {
            return false;
        }

        var error = domainException.Error;
        httpContext.Response.StatusCode = StatusFor(error.Type);

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Title = TitleFor(error.Type),
                Detail = error.Message,
                Status = httpContext.Response.StatusCode,
                Extensions = { ["code"] = error.Code }
            }
        });
    }

    private static int StatusFor(ErrorType type) => type switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Forbidden => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
    };

    private static string TitleFor(ErrorType type) => type switch
    {
        ErrorType.Validation => "Nieprawidłowe żądanie",
        ErrorType.NotFound => "Nie znaleziono",
        ErrorType.Conflict => "Konflikt stanu",
        ErrorType.Forbidden => "Nie znaleziono",
        _ => "Błąd serwera"
    };
}
