using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Controllers;

[ApiController]
[Route("/Admin/[controller]")]
public class BookingController(ILogger<BookingController> logger):ControllerBase
{
    
    [HttpGet()]
    public IActionResult Get()
    {
        logger.LogInformation("Controller Action: Fetching All registerd Bookings  ....");
        return Ok();
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