using System.ComponentModel.DataAnnotations;
using BookingProject.Database;
using BookingProject.Exceptions.DomainExceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookingProject.Validators;

public class BookingMustNotOverlapRule(AppDbContext dbContext): IBookingRule
{
    public bool AppliesTo(BookingValidationOperation operation)
    {
        return operation == BookingValidationOperation.Add || operation == BookingValidationOperation.Update;
    }

    public ValidationError? Validate(Booking bookReq)
    {
        bool isOverLapping=ValidateOverLappingBookingWithAnotherCustomer(bookReq,dbContext.Bookings);

        if (isOverLapping)
        {
            return (
                new ValidationError(
                    Message:" Booking overlapping ",
                    Exp: new CustomExceptions.OverLappingBookingException() )
            );
        }

       
        return (null);
    }
    
    public bool ValidateOverLappingBookingWithAnotherCustomer(Booking booking, IQueryable<Booking> bookings)
    {
        foreach (var bookingRecord in bookings)
        {
            if (booking.Id == bookingRecord.Id)
            {
                continue;
            }
            if (booking.RoomId == bookingRecord.RoomId)
            {
                 if (booking.CheckInDate == bookingRecord.CheckInDate)
                 {
                     return true;
                 }
        
                 if (booking.CheckOutDate <= bookingRecord.CheckOutDate)
                 {
                     return true;
                 }
            }
           
            
        }
        return false;
    }
    
   

    
}