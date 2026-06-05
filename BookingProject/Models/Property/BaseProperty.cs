namespace BookingProject;

public  abstract class BaseProperty
{
    //Data members
    public int Id{get;set;}
    public string Name { get; set; }
    public string? Address { get; set; } 
    public DateTime CreationDate { get; set; } 
    public string? Description { get; set; }
    
    
    // public string? Country { get; set; }
    // public string? City { get; set; }
    // public string? Description { get; set; }
    //aggregate members
    
    public BaseProperty(string name,DateTime creationDate=default)
    {
        this.Name = name;
        this.CreationDate = creationDate;
      
    }
    
}