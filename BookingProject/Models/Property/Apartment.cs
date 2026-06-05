using System.ComponentModel.DataAnnotations;

namespace BookingProject;

public class Apartment : BaseProperty
{
    
    public int BedroomsCount { get; set; }
    public int size { get; set; }
    
    public Apartment(string name, DateTime creationDate) : base(name,creationDate)
    {
    }
}