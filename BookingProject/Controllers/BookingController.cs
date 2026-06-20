using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Controllers;

[ApiController]
[Route("/[controller]")]
public class BookingController(ILogger<BookingController> logger, BookingService bookingService) : ControllerBase
{ 
    [HttpGet("{bookingId:int}")]
    public IActionResult Get(int bookingId)
    {
        logger.LogInformation("Controller Action: Fetching  registered Booking  ....");
        if (bookingId<0)
        {
           return BadRequest("Invalid booking id");
        }
        Booking bookingFetched=bookingService.Get(bookingId)?? throw new CustomExceptions.BookingNotFoundInDbException();
        logger.LogInformation($"Controller Action: Fetching  registered booking with id: {bookingId}  completed.");
        
        return Ok(bookingFetched);
    }
    
    
    [HttpGet()]
    public IActionResult GetAll()
    {
        logger.LogInformation("Controller Action: Fetching All registered Bookings  ....");
        List<Booking> bookingsFetched=bookingService.Get();
        if (bookingsFetched.Count == 0)
        {
            return NotFound("No bookings found");
        }
        
        logger.LogInformation($"Controller Action: Fetching all registered bookings completed.");
        
        return Ok(bookingsFetched);
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