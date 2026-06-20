using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using  BookingProject.Models;
namespace BookingProject;

public class Booking
{
    [Key]
    public int Id {get;set;}=5;
    
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    
    
    //nav properties
    [ForeignKey(nameof(RoomId))]
    public int RoomId { get; set; }
    [JsonIgnore]
    public Room? Room { get; set; }
    
    [ForeignKey(nameof(CustomerId))]
    public int CustomerId{get; set; } 
    [JsonIgnore]
    public Customer? Customer{ get; set; } 
    
    
    
    public enum BookingStatus
    {
        Confirmed,
        Cancelled,
        Pending,
    }
    
}

