using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models.Booking;
using BookingProject.Models.DTO;
using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Controllers;

[ApiController]
[Route("/api/bookings/")]
public class BookingController(ILogger<BookingController> logger, BookingService bookingService) : ControllerBase
{ 
    [HttpGet("{bookingId:Guid}")]
    public IActionResult Get(Guid bookingId)
    {
        logger.LogInformation("Controller Action: Fetching  registered Booking  ....");
       
        Booking bookingFetched=bookingService.Get(bookingId)?? throw new CustomExceptions.BookingNotFoundInDbException();
        logger.LogInformation($"Controller Action: Fetching  registered booking with id: {bookingId}  completed.");
        
        return Ok(bookingFetched);
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        logger.LogInformation("Controller Action: Fetching All registered Bookings in progress ....");
        List<Booking>? bookingsFetched=bookingService.Get();
        if (bookingsFetched is null || bookingsFetched.Count==0)
        {
            logger.LogWarning("Controller Action: Fetching all registered bookings completed. " +
                                  " \t --> No bookings found");
            return NotFound("No bookings found");
        }
        
        logger.LogInformation("Controller Action: Fetching all registered bookings completed.");
        
        return Ok(bookingsFetched);
    }
    
    [HttpDelete("{bookingReqId:Guid}")]
    public IActionResult DeleteBooking( [FromRoute] Guid bookingReqId)
    {
        
        logger.LogInformation("Controller Action:  booking is being deleted .....");
        logger.Log(LogLevel.Information, "Controller Action:  booking is being deleted .....");
        try
        {
            int affectedRows=bookingService.Delete(bookingReqId);
            if (affectedRows>1)
            {
                throw new Exception("More than a booking is deleted");
            }
            if (affectedRows==0)
            {
                throw new DbUpdateException();
            }
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Controller Action: Error while deleting a booking request" +
                            $"   {e.Message}");
            
            return StatusCode(500, new
            {
                message = $"Could not delete booking. {e.Message}, Please try again later.",
            });
        }
        catch (AggregateException e)
        {
    
            logger.LogError("Controller Action: Error while deleting booking" +
                            $" \n Exceptions:  {e.Message}");
            
            var message = e.InnerExceptions.FirstOrDefault()?.Message.Trim()
                          ?? "Booking validation failed.";
            return StatusCode(500, new
            {
                message = $"{message}"
            });
        }
        logger.LogInformation("Controller Action: Booking is Deleted successfully");

        return CreatedAtAction(nameof(DeleteBooking), new
            {
                message = "Booking is Deleted successfully"
            }
        );
        
    }

    
    [HttpPut("{bookingReqId:guid}")]
    public IActionResult UpdateBooking([FromBody] BookingRequestDto bookingReqDto, [FromRoute] Guid bookingReqId)
    {
        
        //parse dates
        HelperFunctions help=new HelperFunctions(logger);
        (DateOnly parsedCheckInDate,DateOnly parsedCheckOutDate) = help.ParseCheckInOutDates(bookingReqDto.CheckInDate, bookingReqDto.CheckOutDate) !;
        if (parsedCheckInDate == default || parsedCheckOutDate == default)
        {
            throw new CustomExceptions.UnsupportedDateTypeValueException();
        }
        // create a raw booking object to send it to the service
        Booking bookingToUpdate = new Booking
        {
            CustomerId = bookingReqDto.CustomerId,
            HotelId = 1,
            RoomId = bookingReqDto.RoomId,
            CheckInDate = parsedCheckInDate,
            CheckOutDate = parsedCheckOutDate,
            NumberOfGuests = bookingReqDto.NumberOfGuests,
            Status = Booking.BookingStatus.Confirmed,
        };

        try
        {
            bookingService.UpdateBooking(bookingToUpdate);

        }
        catch (DbUpdateException e)
        {
            logger.LogError("Controller Action: Error while updating booking:" +
                            $"   {e.Message}");

            return StatusCode(500, new
            {
                message = $"Could not update booking. {e.Message}, Please try again later.",
            });
        }
        catch (AggregateException e)
        {
            logger.LogError("Controller Action: Error while updating  booking" +
                            $" \n Exceptions:  {e.Message}");
            
            var message = e.InnerExceptions.FirstOrDefault()?.Message.Trim()
                          ?? "Booking validation failed.";
            return StatusCode(500, new
            {
                message = $"{message}"
            });
        }
        logger.LogInformation("Controller Action:  Booking is updated successfully");

        return CreatedAtAction(nameof(UpdateBooking), new
            {
                message = "Booking is updated successfully !"
            }
        );
        
    }

    
    [HttpGet("availability")]
    public Task<ActionResult<bool>> CheckRoomAvailability(
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate
    )
    {
        throw new NotImplementedException("Not Implemented yet");
    }
    
    
    
    
}