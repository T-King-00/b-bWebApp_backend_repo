namespace BookingProject.Models.DTO;

public class BookingResponseDto
{
    public Guid Id { get; set; }
    
    public RoomType RoomType { get; set; }

    public DateOnly CheckInDate { get; set; }

    public DateOnly CheckOutDate { get; set; } 
    public int AmountOfNights { get; set; }
    
    public double TotalPrice { get; set; }
    
    public int NumberOfGuests { get; set; }
    
    public string BookingMessage { get; set; }
    
    public Booking.Booking.BookingStatus Status { get; set; }
    
    
}