using System.ComponentModel.DataAnnotations;

namespace BookingProject.Booking;

public class Booking
{
    [Key]
    public int Id {get;set;}
    
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    
    //nav properties
    public int RoomId { get; set; }
    public Room Room { get; set; }
    
    public int CustomerId{get; set; } 
    public Customer Customer{ get; set; } 
    

    
    
    
    
    public enum BookingStatus
    {
        Confirmed,
        Cancelled
    }
    
}

