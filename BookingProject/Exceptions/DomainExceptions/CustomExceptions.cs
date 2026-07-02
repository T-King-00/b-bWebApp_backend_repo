namespace BookingProject.Exceptions.DomainExceptions;

public class CustomExceptions
{
    public sealed class BookingIdDuplicateException:Exception
    {
        public BookingIdDuplicateException(Guid id):base($"There is another booking with this id {id} !")
        { }
    }
    public sealed class InvalidBookingDateException : Exception
    {
        public InvalidBookingDateException()
            : base("Booking Dates are invalid ! ")
        {
        }
    }
    public sealed class InvalidBookingException : Exception
    {
        public InvalidBookingException()
            : base("Booking data is invalid !")
        {
        }
    }
    public sealed class OverLappingBookingException : Exception
    {
        public OverLappingBookingException()
            : base("The same room is  already booked during the requested dates !")
        {
        }
    }
    public sealed class SameCustomerOverLappingBookingException : Exception
    {
        public SameCustomerOverLappingBookingException()
            : base(" You have another booking on same date range !")
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
    
    
    public sealed class  InvalidCustomerData:Exception
    {
        public InvalidCustomerData()
            : base("Customer data is invalid !")
        {
        }
    }

}