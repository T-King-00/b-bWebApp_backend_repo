using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models;
using BookingProject.Models.Booking;
using BookingProject.Models.DTO;
using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Controllers;

[ApiController]
[Route("/api/rooms/{roomId:int}/bookings")]
public class CustomerBookingController(BookingService service,CustomerService customerService,ILogger<CustomerBookingController> logger):ControllerBase
{
    [HttpPost]
    public IActionResult AddNewBooking( int roomId,[FromBody] BookingRequestDto bookingReq)
    {
        logger.LogInformation("Controller Action: New booking is being added .....");
        
        HelperFunctions help=new HelperFunctions(logger);
        (DateOnly parsedCheckInDate,DateOnly parsedCheckOutDate) = help.ParseCheckInOutDates(bookingReq.CheckInDate, bookingReq.CheckOutDate) !;
        BookingResponseDto ? bookingResponseDto;
        try
        {
            if (parsedCheckInDate == default || parsedCheckOutDate == default)
            {
                throw new CustomExceptions.InvalidBookingDateTypeException();
            }
            //check if customer data is available with request or not
            if (bookingReq.CustomerId == Guid.Empty )
            {
                throw new CustomExceptions.InvalidCustomerData();
            }
            

            Booking newBooking = new Booking
            {
                CustomerId = bookingReq.CustomerId,
                HotelId = 1,
                RoomId = roomId,
                CheckInDate = parsedCheckInDate,
                CheckOutDate = parsedCheckOutDate,
                NumberOfGuests = bookingReq.NumberOfGuests,
                Status = Booking.BookingStatus.Confirmed,
            };
            newBooking.Customer=customerService.Get(newBooking.CustomerId);
            if (newBooking.Customer is null)
            {
                throw new CustomExceptions.InvalidCustomerData();
            }
            
            bookingResponseDto = service.Add(newBooking);
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Controller Action: Error while adding new booking" +
                            $"   {e.Message}");
            
            return StatusCode(500, new
            {
                message = $"Could not create booking, Please try again later.",
            });
        }
        catch (AggregateException e)
        {
    
            logger.LogError("Controller Action: Error while adding new booking" +
                            $" \n Exceptions:  {e.Message}");
            
            var message = e.InnerExceptions.FirstOrDefault()?.Message.Trim()
                          ?? "Booking validation failed.";
            return StatusCode(500, new
            {
                message = $"{message}"
            });
        }
        logger.LogInformation("Controller Action: New booking is added successfully");

        return CreatedAtAction(nameof(AddNewBooking), new
            {
                bookingResponseDto,
                message = "New booking is added successfully"
            }
        );
        


    }
    
 
    
    
    
}