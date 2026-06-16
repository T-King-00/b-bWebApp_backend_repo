using BookingProject.Database;
using BookingProject.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Controllers;

public class BookingValidators
{
    public void ValidateDates(DateOnly checkInDate, DateOnly checkOutDate)
    {
        if (checkOutDate <= checkInDate)
        {
            throw new InvalidBookingDateException();
        }
    }
    public void ValidateNotDuplicatedBooking(Booking booking, IQueryable<Booking> bookings)
    {
        
        if (booking.Id != 0 && bookings.Any(existingBooking => existingBooking.Id == booking.Id))
        {
            throw new BookingIdDuplicateException(booking.Id);
        }
    }
}
