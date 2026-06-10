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
    
    public Bed()
    {
    }
    

    public Bed(BedType bedType, int quantity)
    {
        Type = bedType;
        Quantity = quantity;
    }

    public Bed(int bedId, int roomId, BedType bedType, int quantity)
    {   
        this.Id = bedId;
        this.RoomId = roomId;
        this.Type = bedType;
        this.Quantity = quantity;
        
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
