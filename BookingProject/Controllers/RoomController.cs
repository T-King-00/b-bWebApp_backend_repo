using BookingProject.Models;
using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;
namespace BookingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController(ILogger<RoomController> logger, RoomService roomService):ControllerBase
{
    /* api exposed
         * get room by id: to view details of a room : /rooms/{roomId}
         * get available rooms: /rooms
     
     */
    [HttpGet("/rooms")]
    public ActionResult<List<Room>> GetAvailableRooms([FromQuery] DateOnly checkIn, [FromQuery] DateOnly checkOut)
    {
        //var rooms=roomService.GetAllRooms();
        try
        {
            logger.LogInformation("Controller Action: Fetching available rooms in hotelBranch  ....");
            List<Room> rooms=roomService.GetAvailableRooms(1,checkIn, checkOut) 
                             ?? throw new Exception("No rooms found in this hotel branch !");
            return Ok(rooms);
        }
        catch (Exception e)
        {
            logger.LogError(500, e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult GetRoomById(int id)
    {         
        logger.LogInformation("Controller Action: Fetching a room by id  ....");

        try
        {        
            var room=roomService.GetRoomlByIdInHotel(id,1) ?? throw new Exception("No room found with this id !");
            return Ok(room);
        }
        catch (Exception e)
        {
            logger.LogError(500, "  Controller error: No room found with this id!");
            return StatusCode(500, e.Message);
        }

    }
   

    
    

}
