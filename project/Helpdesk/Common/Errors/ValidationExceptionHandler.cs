using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Helpdesk.Common.Errors;

public class ValidationExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Title = "Nieprawidłowe żądanie",
                Detail = "Popraw zaznaczone pola i wyślij formularz ponownie.",
                Status = StatusCodes.Status400BadRequest,
                Extensions =
                {
                    ["code"] = "validation_failed",
                    ["errors"] = ToErrorsByField(validationException)
                }
            }
        });
    }

    private static Dictionary<string, string[]> ToErrorsByField(ValidationException exception) =>
        exception.Errors
            .GroupBy(failure => JsonNamingPolicy.CamelCase.ConvertName(failure.PropertyName))
            .ToDictionary(group => group.Key, group => group.Select(failure => failure.ErrorMessage).ToArray());
}
