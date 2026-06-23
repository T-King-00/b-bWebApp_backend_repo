using System.ComponentModel.DataAnnotations;
using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models.Booking;
using BookingProject.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace BookingProject.Validators;

public interface IBookingRule
{
    bool AppliesTo(BookingValidationOperation operation);
    public ValidationError? Validate(Booking bookReq);

}
public enum BookingValidationOperation
{
    Add,
    Update,
    Delete
}

public sealed record ValidationError(
    string Message,
    Exception Exp
    );

public class ValidationSummary
{
    private readonly List<ValidationError> _errors = new();
    public bool IsValid => !_errors.Any();
    public IEnumerable<ValidationError> Errors => _errors;

    public void AddError(ValidationError error)
    {
        _errors.Add(error);
    }
    
}