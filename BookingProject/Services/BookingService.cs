using BookingProject.Database;
using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Exceptions.Exceptions;
using BookingProject.Models;
using BookingProject.Models.Booking;
using BookingProject.Models.DTO;
using BookingProject.Validators;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Services;

public class BookingService(AppDbContext context,ILogger<BookingService> logger,CompositeValidator bookingValidators,RoomService roomService)
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

    public BookingResponseDto Add(Booking booking)
    {
        
        if (! Validate(booking,BookingValidationOperation.Add))
        {
            return null;
        }
        
       
        if (booking.CreationDateTime == default)
        {
            booking.CreationDateTime = DateTime.UtcNow;
        }

        booking.TotalPrice = CalculateTotalPrice(booking);
        
        context.Bookings.Add(booking);
        
        
        context.SaveChanges() ;
        booking.Room = roomService.GetRoomlByIdInHotelForUpdate(booking.RoomId, booking.HotelId);
        BookingResponseDto bookingResponseDto = new()
        {
            BookingId = booking.Id,
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
    public List<Booking>? Get()
    {
        List<Booking> bookings= context.Bookings.AsNoTracking() .ToList();
        if (bookings.Count==0)
        {
            return null;
        }
        
        return context.Bookings.ToList();
        
    }
    public List<Booking>? GetCustomerBookings(int customerId)
    {
        List<Booking>? bookings= context.Bookings.AsNoTracking().Where(b=>b.CustomerId==customerId) .ToList();
        if (bookings.Count==0 )
        {
            return null;
        }
        
        return context.Bookings.ToList();
        
    }
    

    public int? UpdateBooking(Booking newBooking)
    {
        ArgumentNullException.ThrowIfNull(newBooking);
        
        if (! Validate(newBooking,BookingValidationOperation.Update))
        {
            return null;
        }
        
        // Tracking is on, so EF will pick up changes on SaveChanges.
        var bookingToUpdate = context.Bookings.FirstOrDefault(b=>b.Id==newBooking.Id)?? throw new CustomExceptions.BookingNotFoundInDbException();
        
        bookingToUpdate.CheckInDate = newBooking.CheckInDate;
        bookingToUpdate.CheckOutDate = newBooking.CheckOutDate;
        bookingToUpdate.NumberOfGuests = newBooking.NumberOfGuests;
        bookingToUpdate.ModificationDateTime = DateTime.UtcNow;
        
        //handle the price calculation is missing here.
        bookingToUpdate.TotalPrice = newBooking.TotalPrice;
        
        int affectedRows = context.SaveChanges();
        
        return 
            (affectedRows == 0 ? 
                throw new CustomExceptions.BookingSaveFailedException() : affectedRows);
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
