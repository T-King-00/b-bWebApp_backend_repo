using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Controllers;


[ApiController]
[Route("/api/hotel")]
public class HotelController(HotelService hotelService, ILogger<HotelController> logger):ControllerBase
{
    //return all hotel branches
    [HttpGet("/")]
    public IActionResult GetHotelDetails()
    {
        logger.LogInformation("Controller Action: Fetching  Hotel Details  ....");
        
        //one branch exists with id equal one for now.
        try
        {
            var hotel =hotelService.GetHotelById(1)?? throw new Exception("No hotel found with this id !");
            return Ok(hotel);
        }
        catch (Exception e)
        {
            logger.LogError("   Controller Error: No hotel found with 1 as id !....");
            return StatusCode(500, e.Message);
        }
    }
    
    //return hotel by id
    //future feature.
    /*[HttpGet("{id:int}")]
    public IActionResult GetHotelById(int id)
    {
        return Ok();
    }*/
    
}