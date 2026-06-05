using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController(ILogger<BookingController> logger):ControllerBase
{f
    [HttpGet("/")]
    public IActionResult Get()
    {
        logger.LogInformation("Controller Action: Fetching All registerd Bookings  ....");
        return Ok();
    }
    
    [HttpGet("{id}")]
    public Task<ActionResult<BookingDto>> GetBookingById(int id);
    
    [HttpPost]
    public Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto dto);
    
    [HttpPut("{id}")]
    public Task<IActionResult> UpdateBooking(int id, UpdateBookingDto dto);

    [HttpDelete("{id}")]
    public Task<IActionResult> DeleteBooking(int id);

    [HttpGet("availability")]
    public Task<ActionResult<bool>> CheckRoomAvailability(
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate
    );
    
    
    
    
}