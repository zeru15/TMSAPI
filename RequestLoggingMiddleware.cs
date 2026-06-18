using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        //Generating Correlation Id
        var correlationId = Guid.NewGuid().ToString();

        context.Request.Headers["X-Correlation-Id"] = correlationId;

        var stopwatch = Stopwatch.StartNew();

        //Log Request Start
        _logger.LogInformation(
            "Request {Method} {Path} CorrelationId= {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            correlationId
        );

        // Call the next middleware in the pipeline
        await _next(context);

        //Stop timing
        stopwatch.Stop();

        //Log Request End
        _logger.LogInformation(
            "END StatusCode={StatusCode} Elapsed={ElapsedMilliseconds}ms CorrelationId={CorrelationId}",
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            correlationId
        );
    }
}



