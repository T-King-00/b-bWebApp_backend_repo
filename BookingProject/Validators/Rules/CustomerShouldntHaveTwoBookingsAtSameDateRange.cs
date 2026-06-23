using BookingProject.Database;
using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models.Booking;
using BookingProject.Services;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Validators;

public class CustomerShouldntHaveTwoBookingsAtSameDateRangeRule(AppDbContext dbContext):IBookingRule
{
    public bool AppliesTo(BookingValidationOperation operation)
    {
        if (operation == BookingValidationOperation.Add || operation == BookingValidationOperation.Update)
        {
            return true;
        }
        return false;
    }

    public ValidationError? Validate(Booking bookReq)
    {
        bool  isOverLapping= FindBookingWithSameDateRange( bookReq);
        if (isOverLapping)
        {
            return (
                new ValidationError(
                    Message:"Another Booking is Found At Same Date Range ",
                    Exp: new CustomExceptions.InvalidBookingException()  )
            );
        }
        return (null);
    }
    
    public bool FindBookingWithSameDateRange(Booking bookingReq)
    {
        //fetch customer bookings
        var bookings = dbContext.Bookings.Where(c => c.CustomerId == bookingReq.CustomerId).ToList();
        if (bookings is null)
        {
            return false;
        }
        //check if customer has bookings at same date range
        
        //return true if customer has old booking overlapping with new booking
        if (bookings.Any(b =>
                b.Id != bookingReq.Id &&
                bookingReq.CheckOutDate< b.CheckInDate &&
                bookingReq.CheckInDate > b.CheckOutDate 
                ))
        {
            return true;
        }
        return false;
    }
}
