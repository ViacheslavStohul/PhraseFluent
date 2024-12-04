namespace DistLearning.API.RequestTiming;

using System.Diagnostics;

public class LogRequestTimingMiddleware(RequestDelegate next, ILogger<LogRequestTimingMiddleware> logger)
{
    private static readonly EventId RequestTimingEventId = new(500, "RequestTiming");

    private static readonly Action<ILogger, string?, Exception?> LogMessage =
        LoggerMessage.Define<string?>(LogLevel.Information, RequestTimingEventId, "Request timing info: {LogText}");

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context).ConfigureAwait(false);
        }
        finally
        {
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            LogMessage(logger, $"{elapsedMilliseconds} - {context.Request.Path} - {context.Request.Method}", null);
        }
    }
}