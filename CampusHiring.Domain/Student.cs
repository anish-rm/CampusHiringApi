using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CampusHiring.Api.Common.Enums;

namespace CampusHiring.Api.Domain;

public class Student
{
    // Use the Identity user's Id (string) as both PK and FK to the User table.
    [Key]
    [ForeignKey(nameof(User))]
    public required string UserId { get; set; }

    public User? User { get; set; }

    public int Cgpa { get; set; }
    public int Batch { get; set; }
    public Department Department { get; set; }
    public string ResumeUrl { get; set; } = string.Empty;
    public int CollegeId { get; set; }
    public College? College { get; set; }
}
