using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookingProject;

public  class Hotel : BaseProperty
{
    //Data members
    
    public List<Room> Rooms{get;set;}
    
    
    public Hotel(string name,DateTime creationDate):base(name,creationDate)
    {
      
    }
    public void SetRoomList(List<Room> rooms)
    {
        this.Rooms = rooms;
    }
}
