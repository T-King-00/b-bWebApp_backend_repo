using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookingProject;

public  class Hotel 
{
    //Data members
    public int Id{get;set;}
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Country { get; set; } 
    public string? City { get; set; } 
    public DateTime CreationDate { get; set; } 

    
    //Data members
    public ICollection<Room> Rooms{get;set;}
    
    public Hotel(string name, DateTime creationDate, string city, string country)
    {
        Name = name;
        CreationDate = creationDate;
        City = city;
        Country = country;

    }
    
    public void SetRoomList(List<Room> rooms)
    {
        this.Rooms = rooms;
    }
    
 
}
