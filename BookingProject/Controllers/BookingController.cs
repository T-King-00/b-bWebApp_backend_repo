using BookingProject.Exceptions;
using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Controllers;

[ApiController]
[Route("/[controller]")]
public class BookingController(ILogger<BookingController> logger, BookingService bookingService) : ControllerBase
{

    [HttpGet("/{bookingId:int}")]

public IActionResult Get(int bookingId)
    {
        logger.LogInformation("Controller Action: Fetching All registerd Bookings  ....");
        if (bookingId<0)
        {
            throw new BookingNotFoundInDbException();
        }
        Booking bookingFetched=bookingService.Get(bookingId)?? throw new BookingNotFoundInDbException();
        logger.LogInformation($"Controller Action: Fetching  registered booking with id: {bookingId}  completed.");
        
        return Ok(bookingFetched);
    }

    [HttpGet("{id}")]
    public Task<ActionResult> GetBookingById(int id)
    {
        throw new NotImplementedException("Not Implemented yet");
    }
    
    [HttpPost]
    public Task<ActionResult> CreateBooking()
    {
        throw new NotImplementedException("Not Implemented yet");
    }
    
    [HttpPut("{id}")]
    public Task<IActionResult> UpdateBooking()
    {
        throw new NotImplementedException("Not Implemented yet");
    }

    [HttpDelete("{id}")]
    public Task<IActionResult> DeleteBooking(int id)
    {
        throw new NotImplementedException("Not Implemented yet");
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