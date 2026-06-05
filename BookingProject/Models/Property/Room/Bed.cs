using System.ComponentModel.DataAnnotations.Schema;

namespace BookingProject;

public class Bed
{
    public int Id{get;set;}
    public BedType Type{get;set;}
    public bool Available{get;set;}
    public int Quantity{get;set;}
    
    //nav and reference
    [ForeignKey("RoomId")]

    public Room Room { get; set; } = null!;
    

    public Bed(BedType type, bool available, int quantity)
    {
        Type = type;
        Available = available;
        Quantity = quantity;
    }
    
}

public enum BedType
{
    Single,
    Double,
    King,
    Queen,
    SofaBed,
    BabyCrib
}