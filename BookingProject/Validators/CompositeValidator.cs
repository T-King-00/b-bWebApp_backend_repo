using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Validators;

// contains a list of rules and applies them to the booking
public class CompositeValidator(IEnumerable<IBookingRule> rules)
{
    private readonly IEnumerable<IBookingRule> _rules = rules;


    public  ValidationSummary Validate(Booking bookReq,BookingValidationOperation operation, CancellationToken cancellationToken = default)
    {
        var result = new ValidationSummary();
        
        //filters out rules that dont apply to the operation
        foreach (var rule in _rules.Where(r=>r.AppliesTo(operation)))
        {
            ValidationError ? error =  rule.Validate(bookReq);
            if (error is not null)
            {
                result.AddError(error);
            }
        }
        return result;
    }
    
}
