using Microsoft.AspNetCore.Identity;

namespace CampusHiring.Api.Domain;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt {  get; set; }
    public bool IsActive { get; set; } = true;
}
