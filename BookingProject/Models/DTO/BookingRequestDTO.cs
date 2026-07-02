using System.ComponentModel.DataAnnotations;

namespace BookingProject.Models.DTO;

public class BookingRequestDto
{
   
    [Required]
    public int RoomId { get; set; }
    [Required]
    public string CheckInDate { get; set; }
    [Required]
    public string CheckOutDate { get; set; } 
    
    [Range(1,10)]
    public int NumberOfGuests { get; set; }
    [Required]
    public Guid CustomerId{get;set;}
    
}

