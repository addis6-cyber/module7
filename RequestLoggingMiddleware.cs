using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Generate a short correlation ID
        var correlationId = Guid.NewGuid().ToString("N")[..8];

        // Add it to the response header
        context.Response.Headers["X-Correlation-Id"] = correlationId;

        // Start timing
        var stopwatch = Stopwatch.StartNew();

        // Log the incoming request
        _logger.LogInformation(
            "Request Started | Method: {Method} | Path: {Path} | CorrelationId: {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            correlationId);

        // Pass control to the next middleware
        await _next(context);

        // Stop timing
        stopwatch.Stop();

        // Log the completed request
        _logger.LogInformation(
            "Request Finished | StatusCode: {StatusCode} | Elapsed: {Elapsed} ms | CorrelationId: {CorrelationId}",
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            correlationId);
    }
}