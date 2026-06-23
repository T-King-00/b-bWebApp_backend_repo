using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace BookingProject.Models;


public  class Room
{
    [Key]
    public int Id{get;set;}
    
    public int Size{get;set;}
    public RoomType Type{get;set;}
    public List<Bed> Beds { get; set; } = new ();
    
    
    
    //Navigation property
    public int HotelId { get; set; }
    [JsonIgnore]
    public Hotel Hotel{get;set;}
    public Price Price{get;set;}
    
    [JsonIgnore]
    public Booking.Booking? Booking{get;set;}

    public Room()
    {
    }

    public Room(int hotelId, int roomId, RoomType roomType, int size)
    {
        this.HotelId = hotelId;
        this.Id = roomId;
        this.Type = roomType;
        this.Size = size;
    }
    public Room(RoomType roomType ,int size, List<Bed> beds ,Price basePricePerDay)
    {
        this.Size = size;
        this.Type = roomType;
        this.Beds = beds;
        this.Price = basePricePerDay;
    }
    
}



public enum  RoomType
{
    SingleRoom,
    DoubleRoom,
    SuiteRoom,
    FamilyRoom
}
