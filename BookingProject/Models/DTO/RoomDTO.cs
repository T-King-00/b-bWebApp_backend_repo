namespace BookingProject.Models.DTO;

public class RoomDto
{
    
    public int Id{get;set;}
    public int Size{get;set;}
    public RoomType Type{get;set;}
    public int MaxGuestsAmount { get; set; }
    public double TotalPrice{get;set;}
    public double BasePrice{get;set;}
    
    

}