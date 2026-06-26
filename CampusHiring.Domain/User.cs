using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusHiring.Api.Domain;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt {  get; set; }
    public bool IsActive { get; set; } = true;
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
