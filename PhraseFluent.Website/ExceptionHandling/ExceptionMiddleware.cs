using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security;
using Microsoft.IdentityModel.Tokens;
using PhraseFluent.Service.Exceptions;

namespace PhraseFluent.API.ExceptionHandling;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private const int GenericExceptionEventId = 10000;
    private static readonly Action<ILogger, Exception> GenericError =
        LoggerMessage.Define(LogLevel.Error, new EventId(GenericExceptionEventId), "Error occured.");
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            GenericError(logger, ex);
            
            context.Response.StatusCode = ex switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                SecurityTokenValidationException => (int)HttpStatusCode.BadRequest,
                DataNotFoundException => (int)HttpStatusCode.NotFound,
                ForbiddenException => (int)HttpStatusCode.Forbidden,
                _ => (int)HttpStatusCode.InternalServerError
            };

            await CreateExceptionResponseAsync(context, ex);
        }
    }

    private static Task CreateExceptionResponseAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = context.Response.StatusCode == (int)HttpStatusCode.InternalServerError ? "Unexpected server error" : ex.Message
        }.ToString());
    }
}