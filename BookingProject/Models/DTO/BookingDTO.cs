using System.ComponentModel.DataAnnotations;

namespace BookingProject.Models.DTO;

public class BookingRequestDTO
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
    public CustomerDTO Customer{get;set;}
    
}

public class CustomerDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}