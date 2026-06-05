using Microsoft.AspNetCore.Mvc;
namespace BookingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController(ILogger<RoomController> logger):ControllerBase
{
    [HttpGet()]
    public IActionResult GetAllRooms()
    {
        logger.LogInformation("Controller Action: Fetching All registerd rooms  ....");
        
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetRoomById(Guid id)
    {
        logger.LogInformation("Controller Action: Fetching a room by id  ....");
        return Ok();
    }
    
    

}