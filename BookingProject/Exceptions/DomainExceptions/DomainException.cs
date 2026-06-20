namespace BookingProject.Exceptions.DomainExceptions;

public class DomainException: Exception
{
    public IReadOnlyList<string> Errors { get; }
    // Single error
    public DomainException(string error) : base(error) => Errors = new List<string> { error };
    
    public DomainException(IEnumerable<string> errors):base("One or more business rules were violated.")
    {
        Errors = errors.ToList();
    } 
}