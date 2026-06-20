using System.Data;
using BookingProject.Controllers;
using BookingProject.Database;
using BookingProject.Exceptions;
using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Validators;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Services;

public class BookingService(AppDbContext context,ILogger<BookingService> logger,CompositeValidator _bookingValidators)
{
 
   
    public bool ValidateForAdd(Booking bookingRequest)
    {
        BookingValidationOperation operation=BookingValidationOperation.Add;
        
        var result=_bookingValidators.Validate(bookingRequest,operation);

        if (!result.IsValid)
        {   
            logger.LogError(result.Errors.ToString());
            var errors = result.Errors.Select(e => e.Exp).ToList();

            if (errors.Any())
            {
                throw new AggregateException("Multiple validation errors occurred.",errors);
            }
            
        }
        return result.IsValid ;

    }
    public bool ValidateForUpdate(Booking bookingRequest)
    {
        var result=_bookingValidators.Validate(bookingRequest,BookingValidationOperation.Update);

        if (!result.IsValid)
        {   
            logger.LogError(result.Errors.ToString());
            foreach (var err in result.Errors)
            {
                throw  err.Exp;
            }
            
        }
        return result.IsValid ;

    }
    public Booking Add(Booking booking)
    {
        if (! ValidateForAdd(booking))
        {
            return null;
        }
        
       
        if (booking.CreationDateTime == default)
        {
            booking.CreationDateTime = DateTime.UtcNow;
        }

        context.Bookings.Add(booking);
        context.SaveChanges() ;
        return booking;
    }
    public Booking Get(int bookingId)
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
        List<Booking> ? bookings= context.Bookings.AsNoTracking() .ToList();
        if (bookings.Count==0)
        {
            return null;
        }
        
        return context.Bookings.ToList();
        
    }

    public int UpdateBooking(Booking newBooking)
    {
        ArgumentNullException.ThrowIfNull(newBooking);
        
        if (! ValidateForUpdate(newBooking))
        {
            throw new CustomExceptions.BookingSaveFailedException();
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

    public int Delete(int bookingId)
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
    
}
