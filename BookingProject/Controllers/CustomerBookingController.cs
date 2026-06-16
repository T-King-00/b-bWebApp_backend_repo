using BookingProject.Database;
using BookingProject.Exceptions;
using BookingProject.Models;
using BookingProject.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Controllers;

[ApiController]
[Route("/rooms/{roomId:int}/bookingForm")]
public class CustomerBookingController(AppDbContext _DbContext,ILogger<CustomerBookingController> logger):ControllerBase
{
    [HttpPost]
    public ActionResult AddNewBooking( int roomId,[FromBody] BookingRequestDTO bookingReq)
    {
        logger.LogInformation("Controller Action: New booking is being added .....");
        
        HelperFunctions help=new HelperFunctions(logger);
        (DateOnly parsedCheckInDate,DateOnly parsedCheckOutDate) = help.ParseCheckInOutDates(bookingReq.CheckInDate, bookingReq.CheckOutDate) !;

        try
        {
            if (parsedCheckInDate == default || parsedCheckOutDate == default)
            {
                throw new InvalidBookingDateTypeException();
            }

            Booking newBooking = new Booking
            {
                Id = 2,
                CustomerId = 1,
                Customer = new Customer
                {
                    FirstName = bookingReq.Customer.FirstName,
                    LastName = bookingReq.Customer.LastName,
                    Email = bookingReq.Customer.Email,
                    PhoneNumber = bookingReq.Customer.PhoneNumber
                },
                RoomId = roomId,
                CheckInDate = parsedCheckInDate,
                CheckOutDate = parsedCheckOutDate,
                NumberOfGuests = bookingReq.NumberOfGuests,
            };

            _DbContext.Bookings.Add(newBooking);
            _DbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Controller Action: Error while adding new booking" +
                            $"   {e.Message}");

            return StatusCode(500, new
            {
                message = "Could not create booking. Please try again later."
            });
        }

        logger.LogInformation("Controller Action: New booking is added successfully");
           
        
        return CreatedAtAction(nameof(AddNewBooking), new
        {
            message = "New booking is added successfully"
        });
       
        
    }
    
    
}