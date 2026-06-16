using BookingProject.Controllers;
using BookingProject.Database;
using BookingProject.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Services;

public class BookingService(AppDbContext context)
{
    private readonly BookingValidators _bookingValidators = new();

    public Booking Get(int bookingId)
    {
        Booking? bookingToFetch=context.Bookings.AsNoTracking().FirstOrDefault(b => b.Id == bookingId);
        if (bookingToFetch is null)
        {
            throw new BookingNotFoundInDbException();
        }
        return bookingToFetch;


    }
    public Booking Add(Booking booking)
    {
        ArgumentNullException.ThrowIfNull(booking);

        _bookingValidators.ValidateDates(booking.CheckInDate, booking.CheckOutDate);
        _bookingValidators.ValidateNotDuplicatedBooking(booking, context.Bookings );
        
        if (booking.CreationDateTime == default)
        {
            booking.CreationDateTime = DateTime.UtcNow;
        }

        context.Bookings.Add(booking);
        context.SaveChanges();
        return booking;
    }

    public int UpdateBooking(Booking newBooking)
    {
        ArgumentNullException.ThrowIfNull(newBooking);
        
        _bookingValidators.ValidateDates(newBooking.CheckInDate, newBooking.CheckOutDate);
        
        // Tracking is on, so EF will pick up changes on SaveChanges.
        var bookingToUpdate = context.Bookings.FirstOrDefault(b=>b.Id==newBooking.Id)?? throw new BookingNotFoundInDbException();
        
        bookingToUpdate.CheckInDate = newBooking.CheckInDate;
        bookingToUpdate.CheckOutDate = newBooking.CheckOutDate;
        bookingToUpdate.NumberOfGuests = newBooking.NumberOfGuests;
        bookingToUpdate.ModificationDateTime = DateTime.UtcNow;
        
        //handle the price calculation is missing here.
        bookingToUpdate.TotalPrice = newBooking.TotalPrice;
        
        int affectedRows = context.SaveChanges();
        
        return 
            (affectedRows == 0 ? 
                throw new BookingSaveFailedException() : affectedRows);
    }

    public int Delete(int bookingId)
    { var bookingItem=context.Bookings.Find(bookingId);
        if (bookingItem is  null)
        {
            throw new BookingNotFoundInDbException();
        }
       
        context.Bookings.Remove(bookingItem);
        var affectedRows=context.SaveChanges();
        return 
            (affectedRows == 0 ? 
                throw new BookingSaveFailedException() : affectedRows);
      
        
    }
    
}
