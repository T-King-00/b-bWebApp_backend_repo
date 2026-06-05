using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingProject;

public  class Room
{
    [Key]
    public int Id{get;set;}
    
    public int size{get;set;}
    public RoomType RoomType{get;set;}
    public List<Bed> Beds { get; set; } = new ();
    
    
    
    //Navigation property
    public Hotel Hotel{get;set;}
    
    public Price Price{get;set;}
    
    
    
    public Room()
    {
        
    }
    public Room(int size, RoomType roomType, List<Bed> beds ,Price basePricePerDay)
    {
        this.size = size;
        this.RoomType = roomType;
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
