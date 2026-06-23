using BookingProject.Models;
using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;
using BookingProject.Models.DTO;
using BookingProject.Validators;

namespace BookingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController(ILogger<RoomController> logger, RoomService roomService):ControllerBase
{
 
    [HttpGet("/rooms")]
    public ActionResult<List<RoomDto>> GetAvailableRooms([FromQuery] string checkInDate, [FromQuery] string checkOutDate,[FromQuery] int ? numberOfGuests)
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
                logger.LogError("Controller Action: Invalid dates format");
                return BadRequest("Invalid dates");
            }

            DateValidationRule dateValidationRule=new DateValidationRule();
            bool isValidDates=dateValidationRule.ValidateBookingDates(parsedCheckInDate, parsedCheckOutDate);
            if (!isValidDates)
            {
                logger.LogWarning("Controller Action: DateValidationRule failed");
                return BadRequest("Invalid dates");
            }
            
            //no room found is not an exception, it is a normal flow(can happen)
            List<RoomDto> rooms=roomService.GetAvailableRooms(1, parsedCheckInDate, parsedCheckOutDate, numberOfGuests) ! ;

            if (rooms is null)
            {
                return NotFound("No rooms available for the selected dates");
            }
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
            
            List<RoomDto> roomsDto=roomService.GetAll(1) 
                             ?? throw new Exception("No rooms found in this hotel branch !");
            return Ok(roomsDto);
        }
        catch (Exception e)
        {
            logger.LogError(500, e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("/rooms/{id:int}")]
    public IActionResult GetRoomById(int id,[FromQuery] string checkInDate, [FromQuery] string checkOutDate)
    {         
        logger.LogInformation("Controller Action: Fetching a room by id  ....");
        
        try
        {        
            //step1: date parsing
            HelperFunctions help=new HelperFunctions(logger);
            (DateOnly parsedCheckInDate,DateOnly parsedCheckOutDate)  = help.ParseCheckInOutDates(checkInDate, checkOutDate)!;
            if ( parsedCheckInDate ==default|| parsedCheckOutDate==default )
            {
                logger.LogError("Controller Action: Invalid dates format");
                return BadRequest("Invalid dates");
            }
            RoomService.DateRange dateRange=new RoomService.DateRange(parsedCheckInDate, parsedCheckOutDate);

            DateValidationRule dateValidationRule=new DateValidationRule();
            bool isValidDates=dateValidationRule.ValidateBookingDates(dateRange.CheckInDate, dateRange.CheckOutDate);
            if (!isValidDates)
            {
                logger.LogWarning("Controller Action: DateValidationRule failed");
                return BadRequest("Invalid dates");
            }
            //step2: room fetching
            Room reqRoomInHotel=new Room();
            reqRoomInHotel.Id = id;
            reqRoomInHotel.HotelId = 1;
            
            var room=roomService.GetRoomlByIdInHotel(reqRoomInHotel,dateRange) ?? throw new Exception("No room found with this id !");
            //step3: add total price
            
            logger.LogInformation("Controller Action: Fetching a room by id completed.");
            return Ok(room);
        }
        catch (Exception e)
        {
            logger.LogError(500, "  Controller error: No room found with this id!");
            return StatusCode(500, e.Message);
        }

    }
    

}
