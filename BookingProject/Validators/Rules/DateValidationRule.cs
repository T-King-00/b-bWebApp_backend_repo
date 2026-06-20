using System.ComponentModel.DataAnnotations;
using BookingProject.Controllers;
using BookingProject.Exceptions.DomainExceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Validators;

public class DateValidationRule: IBookingRule
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
        bool  isValidDates= ValidateBookingDates(bookReq.CheckInDate, bookReq.CheckOutDate);
        if (!isValidDates)
        {
            return (
                new ValidationError(
                    Message:"Check-in date must be before check-out date & Check-in cant be today's date ",
                    Exp: new CustomExceptions.InvalidBookingDateException()  )
                );
        }
        return (null);
    }
    

    public bool ValidateBookingDates(DateOnly checkInDate, DateOnly checkOutDate)
    {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;

        var todaysDate = new DateOnly(year, month, day);

        if (checkInDate <= todaysDate || checkOutDate <= checkInDate)
        {
            return false;
        }
        return true;
    }


 
}