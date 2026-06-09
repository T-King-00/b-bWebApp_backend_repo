using System.ComponentModel.DataAnnotations.Schema;

namespace BookingProject;

public class Bed
{
    public int Id{get;set;}
    public BedType Type{get;set;}
    public int Quantity{get;set;}
    
    //nav and reference
    [ForeignKey("RoomId")]
    public int RoomId{get;set;}
    
    
    

    public Bed(BedType type, int quantity)
    {
        Type = type;
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