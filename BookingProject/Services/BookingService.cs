using BookingProject.Database;
using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models;
using BookingProject.Models.Booking;
using BookingProject.Models.DTO;
using BookingProject.Validators;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Services;

public class BookingService(AppDbContext context,ILogger<BookingService> logger,CompositeValidator bookingValidators,RoomService roomService,CustomerService customerService)
{
    
    private bool Validate(Booking bookingRequest,BookingValidationOperation operation)
    {
        var result=bookingValidators.Validate(bookingRequest,operation);

        if (!result.IsValid)
        {   
            var errors = result.Errors.Select(e => e.Exp).ToList();
            if (errors.Any())
            {
                throw new AggregateException("Multiple validation errors occurred.",errors);
            }
            
        }
        return result.IsValid ;

    }
   
    /// <summary>
    /// Add function adds a booking to the database.
    /// First, it checks if the customer is already in the database, if not, it adds the customer to the database.
    /// Then, it calculates the total price of the booking.
    /// Secondly, it sets the customer id and nulls booking.Customer variable to avoid loops that occurs due to navigation properties.
    /// </summary>
    /// <param name="booking"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="RoomNotFoundException"></exception>
    /// <exception cref="CustomExceptions.InvalidBookingException"></exception>

    public BookingResponseDto Add(Booking booking)
    {
        //checks for nulls first,throw exception if any nulls found
        ArgumentNullException.ThrowIfNull(booking);
        ArgumentNullException.ThrowIfNull(booking.Customer);
        
        logger.LogInformation(booking.Customer.FirstName + " " + booking.Customer.LastName);
        
        // check if customer is already in the database by using id
        Customer? customer   =customerService.Get(booking.Customer.Id) ;
        //if not, add customer to database
        if (customer is null)
        {
            customer=booking.Customer;
            context.Customers.Add(booking.Customer);
        }
        
        booking.CustomerId = customer.Id;
        //nulling the customer object to avoid loops that occurs due to navigation properties.
        booking.Customer = null;
        
        //Validate function already throws on exceptions
        Validate(booking, BookingValidationOperation.Add);
      
        if (booking.CreationDateTime == default)
        {
            booking.CreationDateTime = DateTime.UtcNow;
        }
        // initialize the total price  
        booking.TotalPrice = CalculateTotalPrice(booking);
        
        context.Bookings.Add(booking);
        context.SaveChanges() ;
        
        booking.Room = roomService.GetRoomlByIdInHotelForUpdate(booking.RoomId, booking.HotelId)
            ?? throw new RoomNotFoundException(booking.RoomId);
        BookingResponseDto bookingResponseDto = new()
        {
            Id = booking.Id,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            NumberOfGuests = booking.NumberOfGuests,
            RoomType = booking.Room.Type,
            TotalPrice =  booking.TotalPrice,
            Status = booking.Status,
            BookingMessage = "Booking added successfully",

        };
        
        return bookingResponseDto;
    }
    
    public Booking Get(Guid bookingId)
    {
        Booking? bookingToFetch=context.Bookings.AsNoTracking().FirstOrDefault(b => b.Id == bookingId);
        if (bookingToFetch is null)
        {
            throw new CustomExceptions.BookingNotFoundInDbException();
        }
        return bookingToFetch;


    }
    public List<Booking> Get()
    {
        List<Booking> bookings= context.Bookings.AsNoTracking() .ToList();
        if (bookings.Count==0)
        {
            return new List<Booking>();
        }
        
        return context.Bookings.ToList();
        
    }
    public List<Booking> GetCustomerBookings(Guid customerId)
    {
        List<Booking> customerBookings= context.Bookings.AsNoTracking().Where(b=>b.CustomerId==customerId) .ToList();
        return customerBookings;
    }
    

    public int UpdateBooking(Booking newBookingReq )
    {
        ArgumentNullException.ThrowIfNull(newBookingReq);

        Validate(newBookingReq, BookingValidationOperation.Update);
        
        // Tracking is on, so EF will pick up changes on SaveChanges.
        var bookingToUpdate = context.Bookings.FirstOrDefault(b=>b.Id==newBookingReq.Id)??
                              throw new CustomExceptions.BookingNotFoundInDbException();
        
        bookingToUpdate.CheckInDate = newBookingReq.CheckInDate;
        bookingToUpdate.CheckOutDate = newBookingReq.CheckOutDate;
        bookingToUpdate.NumberOfGuests = newBookingReq.NumberOfGuests;
        bookingToUpdate.RoomId = newBookingReq.RoomId;
        bookingToUpdate.ModificationDateTime = DateTime.UtcNow;
        
        //handle the price calculation is missing here.
        bookingToUpdate.TotalPrice = CalculateTotalPrice(newBookingReq);
        
        int affectedRows = context.SaveChanges();
        if (affectedRows == 0)
        {
            throw new CustomExceptions.BookingSaveFailedException();
        }

        return affectedRows;
    }

    public int Delete(Guid bookingId)
    { var bookingItem=context.Bookings.Find(bookingId);
        if (bookingItem is  null)
        {
            throw new CustomExceptions.BookingNotFoundInDbException();
        }
       
        context.Bookings.Remove(bookingItem);
        var affectedRows=context.SaveChanges();
        return 
            (affectedRows == 0 ? 
                throw new CustomExceptions.BookingSaveFailedException() : affectedRows);
      
        
    }

    private Double CalculateTotalPrice(Booking booking)
    {
        var roomPrice=context.Rooms.Select(r=>r.Price).FirstOrDefault(r => r.Id==booking.RoomId);

        if (roomPrice is null)
        {
            throw new RoomNotFoundException(booking.RoomId);
        }
        
        int countNumberOfNights=booking.CheckOutDate.DayNumber -booking.CheckInDate.DayNumber;
        if (countNumberOfNights <= 0)
        {
            throw new CustomExceptions.InvalidBookingException();
        }
        Double totalPrice = roomPrice.BasePrice*countNumberOfNights;
        
        return totalPrice;
        
    }
    
}
