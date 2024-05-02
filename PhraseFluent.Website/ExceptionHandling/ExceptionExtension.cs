namespace PhraseFluent.API.ExceptionHandling;

public static class ExceptionExtension
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app) => app.UseMiddleware<ExceptionMiddleware>();
}