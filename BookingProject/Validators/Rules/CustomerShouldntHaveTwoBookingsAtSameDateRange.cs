using BookingProject.Database;
using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models.Booking;
using BookingProject.Services;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Validators;


/// <summary>
///  Customer must not have overlapping bookings at same date range.
/// </summary>
/// <param name="dbContext"></param>
/// 
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
                    Exp: new CustomExceptions.SameCustomerOverlappingBookingException()  )
            );
        }
        return (null);
    }
    
    public bool FindBookingWithSameDateRange(Booking bookingReq)
    {
        //fetch customer bookings
        //check if customer has old valid bookings at same date range
        //return true if customer has old booking overlapping with new booking
        if (dbContext.Bookings.Any(b =>
                b.CustomerId == bookingReq.CustomerId &&
                b.Id != bookingReq.Id &&
                ( bookingReq.CheckOutDate> b.CheckInDate &&
                  bookingReq.CheckInDate < b.CheckOutDate )
                ))
        {
            return true;
        }
        return false;
    }
}
