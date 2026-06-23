using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models.Booking;
using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Controllers;

[ApiController]
[Route("/[controller]")]
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
    
    
    [HttpGet()]
    public IActionResult GetAll()
    {
        logger.LogInformation("Controller Action: Fetching All registered Bookings  ....");
        List<Booking>? bookingsFetched=bookingService.Get();
        if (bookingsFetched is null || bookingsFetched.Count==0)
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
    
    [HttpPut("{Guid}")]
    public Task<IActionResult> UpdateBooking()
    {
        throw new NotImplementedException("Not Implemented yet");
    }

    [HttpDelete("{Guid}")]
    public Task<IActionResult> DeleteBooking(Guid id)
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