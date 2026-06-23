using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookingProject.Models.Booking;

public class Booking
{
    [Key]
    public Guid Id {get;set;}=Guid.NewGuid();
    
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }
    public int NumberOfGuests { get; set; }
    public double TotalPrice { get; set; }
    
    
    //nav properties
    [ForeignKey(nameof(RoomId))]
    public int RoomId { get; set; }
    [JsonIgnore]
    public Room? Room { get; set; }
    
    [ForeignKey(nameof(HotelId))]
    public int HotelId { get; set; }
    [JsonIgnore]
    public Hotel? Hotel { get; set; }
    
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

