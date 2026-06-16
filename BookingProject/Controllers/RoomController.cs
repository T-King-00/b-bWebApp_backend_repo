using BookingProject.Models;
using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;

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
    public ActionResult<List<Room>> GetAvailableRooms([FromQuery] string checkInDate, [FromQuery] string checkOutDate)
    {
        //var rooms=roomService.GetAllRooms();
        HelperFunctions help=new HelperFunctions(logger);
        try
        {
            
            logger.LogInformation("Controller Action: Fetching available rooms in hotelBranch  ....");
            logger.LogInformation($"Controller Action: within dates {checkInDate} + {checkOutDate} ....");
            (DateOnly parsedCheckInDate,DateOnly parsedCheckOutDate)  = help.ParseCheckInOutDates(checkInDate, checkOutDate)!;
            if ( parsedCheckInDate ==default|| parsedCheckOutDate==default )
            {
                logger.LogError("Controller Action: Invalid dates");
                return BadRequest("Invalid dates");
            }

            if (parsedCheckOutDate <= parsedCheckInDate)
            {
                logger.LogWarning("Controller Action: checkOutDate must be after checkInDate.");
                return BadRequest("checkOutDate must be after checkInDate.");
            }

          List<Room> rooms=roomService.GetAvailableRooms(1, parsedCheckInDate, parsedCheckOutDate) 
                             ?? throw new Exception("No rooms found in this hotel branch !");
            return Ok(rooms);
        }
        catch (Exception e)
        {
            logger.LogError(500, e.Message);
            return StatusCode(500, e.Message);
        }
    }
    [HttpGet("/allRooms")]
    public ActionResult<List<Room>> GetAvailableRooms()
    {
        try
        {
            logger.LogInformation("Controller Action: Fetching All  rooms in hotelBranch  ....");
            List<Room> rooms=roomService.GetAll(1) 
                             ?? throw new Exception("No rooms found in this hotel branch !");
            return Ok(rooms);
        }
        catch (Exception e)
        {
            logger.LogError(500, e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("/rooms/{id:int}")]
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
