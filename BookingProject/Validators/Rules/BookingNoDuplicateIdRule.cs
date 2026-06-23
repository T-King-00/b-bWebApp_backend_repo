using BookingProject.Database;
using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models.Booking;

namespace BookingProject.Validators;

public class BookingNoDuplicateIdRule(AppDbContext dbContext):IBookingRule
{
    public bool AppliesTo(BookingValidationOperation operation)
    {
       return operation==BookingValidationOperation.Add;
    }

    public ValidationError? Validate(Booking bookReq)
    {
        if (ValidateNotDuplicatedBooking(bookReq,dbContext.Bookings))
        {
            return new ValidationError(
                Message: "Duplicate booking found! ",
                Exp: new CustomExceptions.BookingIdDuplicateException(bookReq.Id));

        }
        return null;
    }
    public bool ValidateNotDuplicatedBooking(Booking booking, IQueryable<Booking> bookings)
    {
            
        if ( bookings.Any(existingBooking => existingBooking.Id == booking.Id))
        {
            return true;
        }
        return false; 
    }


}