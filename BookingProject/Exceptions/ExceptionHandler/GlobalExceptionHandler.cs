using BookingProject.Exceptions.DomainExceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Exceptions.ExceptionHandler;

/// <summary>
///Instead of using try catch blocks everywhere, global exception handler middleware centralizes the logic.
/// Global exception handler handles exceptions
/// that are not thrown or handled instead of letting the application fail unexpectedly.
/// </summary>
/// <param name="_logger"></param>



public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger):IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;
        _logger.LogError(exception, $"!!! Unhandled Exception:-> TraceId:{traceId}. Message: {exception.Message}" );
        
        //Frontend uses this to display error message
        var problemDetails = exception switch
        {
            CustomExceptions.BookingNotFoundInDbException=> CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status404NotFound,
                    "Booking not found.",
                    exception.Message),
            
            RoomNotFoundException=> CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status404NotFound,
                    "Room not found.",
                    exception.Message),
            
            CustomExceptions.InvalidBookingDateException
                or CustomExceptions.InvalidBookingException
                or CustomExceptions.InvalidCustomerData => CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status400BadRequest,
                    "Invalid request",
                    exception.Message),
            
            CustomExceptions.UnsupportedDateTypeValueException => CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status400BadRequest,
                    "Invalid request",
                    "Date type is not supported!"),
            
            CustomExceptions.BookingIdDuplicateException
                or CustomExceptions.OverlappingBookingException
                or CustomExceptions.SameCustomerOverlappingBookingException => CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status409Conflict,
                    "Booking conflict",
                    exception.Message),
            
            _ => CreateProblemDetails(
                httpContext,
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "Something went wrong. Please try again later.")

            

        };
        // add extra field to json response
        problemDetails.Extensions["traceId"] = traceId;
        //this line makes the real HTTP response match the error body
        httpContext.Response.StatusCode =
            problemDetails.Status ?? problemDetails.Status!.Value;
        
        
        if (problemDetails.Status >= 500)
        {
            _logger.LogError(exception, "Unhandled exception. TraceId: {TraceId}", traceId);
        }
        else
        {
            _logger.LogWarning(exception, "Handled application exception. TraceId: {TraceId}", traceId);
        }
        
        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);
        
        //means exception is thrown successfully
        return true;
    }

    private ProblemDetails CreateProblemDetails(HttpContext httpContext, int statusCode, string title, string detail)
    {
        return new ProblemDetails()
        {
            Status= statusCode,
            Title= title,
            Detail= detail ,
            Instance = httpContext.Request.Path
        };

    }
}