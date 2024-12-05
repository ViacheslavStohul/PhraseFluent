namespace DistLearning.API.RequestTiming;

public static class LogRequestTimingExtension
{
    public static IApplicationBuilder UseLoggingTimingCalculation(this IApplicationBuilder app) => app.UseMiddleware<LogRequestTimingMiddleware>();
}