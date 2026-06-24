using CampusHiring.Api.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace CampusHiring.Api.Application.DTOs.Auth;

public class RegisterUserDto : IValidatableObject
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(8)]
    public required string Password { get; set; }
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    public string Role {  get; set; } = RoleNames.Student;
    public int? AssociatedCollegeId { get; set; }
    public int? AssociatedCompanyId { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Role == RoleNames.Student && AssociatedCollegeId.GetValueOrDefault() < 1)
        {
            yield return new ValidationResult("Please Provide valid College Id", [nameof(AssociatedCollegeId)]);
        }
        if (Role == RoleNames.Interviewer && AssociatedCompanyId.GetValueOrDefault() < 1)
        {
            yield return new ValidationResult("Please Provide valid Company Id", [nameof(AssociatedCompanyId)]);
        }
        if (Role == RoleNames.CollegeAdmin && AssociatedCollegeId.GetValueOrDefault() < 1)
        {
            yield return new ValidationResult("Please Provide valid College Id", [nameof(AssociatedCollegeId)]);
        }
    }
}
