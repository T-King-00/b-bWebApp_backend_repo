using System.ComponentModel.DataAnnotations;
using BookingProject.Database;
using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models.Booking;
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
                    Exp: new CustomExceptions.OverlappingBookingException() )
            );
        }

       
        return (null);
    }
    
    public bool ValidateOverLappingBookingWithAnotherCustomer(Booking booking, IQueryable<Booking> bookings)
    {
        bool isOverLapping = false;
        foreach (var bookingRecordToCompareWith in bookings)
        {
            if (booking.Id == bookingRecordToCompareWith.Id)
            {
                continue;
            }
            if (booking.RoomId == bookingRecordToCompareWith.RoomId)
            {
                 if (booking.CheckInDate > bookingRecordToCompareWith.CheckOutDate && booking.CheckOutDate >= bookingRecordToCompareWith.CheckInDate)
                 {
                     isOverLapping=false;
                 }
                 else
                 {
                     isOverLapping=true;
                 }
            }
        }
        return isOverLapping;
    }
    
   

    
}