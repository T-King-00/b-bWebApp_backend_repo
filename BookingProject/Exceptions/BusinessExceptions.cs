using System ;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

// These exceptions describe business errors.
namespace BookingProject.Exceptions
    {
    public sealed class RoomNotFoundException: Exception
    {
        public RoomNotFoundException(int id):base($"Room with id {id} not found !")
        { }
    }

    public sealed class BookingIdDuplicateException:Exception
    {
        public BookingIdDuplicateException(int id):base($"There is another booking with this id {id} !")
        { }
    }
    public sealed class InvalidBookingDateException : Exception
    {
        public InvalidBookingDateException()
            : base("Check-out date must be after check-in date !")
        {
        }
    }
    public sealed class InvalidBookingDateTypeException : Exception
    {
        public InvalidBookingDateTypeException()
            : base("Date type is not supported ! ")
        {
        }
    }
    public sealed class BookingNotFoundInDbException : Exception
    {
        public BookingNotFoundInDbException()
            : base("Booking not found ! ")
        {
        }
    }

    public sealed class BookingSaveFailedException : Exception
    {
        public BookingSaveFailedException()
            : base("Booking updates could not be saved to the database.")
        {
        }
    }
    

    }
