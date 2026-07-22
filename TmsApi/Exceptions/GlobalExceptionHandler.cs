using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TmsApi.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        ProblemDetails problem;

        switch (exception)
        {
            case ValidationException validationException:

                problem = new ValidationProblemDetails(
                    validationException.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()))
                {
                    Title = "Validation failed",
                    Status = StatusCodes.Status400BadRequest
                };

                httpContext.Response.StatusCode =
                    StatusCodes.Status400BadRequest;

                break;

            default:

                problem = new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = exception.Message,
                    Status = StatusCodes.Status500InternalServerError
                };

                httpContext.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                break;
        }

        await httpContext.Response.WriteAsJsonAsync(
            problem,
            cancellationToken);

        return true;
    }
}