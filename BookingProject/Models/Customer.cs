using System.ComponentModel.DataAnnotations;
using BookingProject;
namespace BookingProject.Models;

/// <summary>
/// Represents a customer who can make one or more bookings.
/// </summary>
public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [MaxLength(15)]
    public string? PhoneNumber { get; set; }

    [MaxLength(12)]
    public string? PersonalNumber { get; set; }

    public ICollection<Booking> Bookings { get; set; } = [];
}
