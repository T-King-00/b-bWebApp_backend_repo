using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Exceptions.ExceptionHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger):IExceptionHandler
{
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;
        _logger.LogError(exception, $"!!! Unhandled Exception:-> TraceId:{traceId}. Message: {exception.Message}" );
        
        //Frontend uses this to display error message
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = "Something went wrong. Please try again later.",
            Instance = httpContext.Request.Path,
        };
        // add extra field to json response
        problemDetails.Extensions["traceId"] = traceId;
        //this line makes the real HTTP response match the error body
        httpContext.Response.StatusCode =
            problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        
        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);
        
        //means exception is thrown successfully
        return true;
    }
}