using BookingProject.Models.Booking;
using Microsoft.AspNetCore.Identity;

namespace BookingProject.Controllers.Identity;


/// <summary>
/// <see cref="User"/> class adds to IdentityUser class application-specific data.
/// </summary>
public class User : IdentityUser
{
    public string FirstName { get; set; }=string.Empty;
    public string LastName { get; set; }=string.Empty;
    public List<Booking> Booking { get; set; } = new List<Booking>();
    



}