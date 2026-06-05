using System.ComponentModel.DataAnnotations;

namespace BookingProject;

public class Villa : BaseProperty
{
 
    public Villa(string name,DateTime creationDate) : base(name,creationDate)
    {
    }
}