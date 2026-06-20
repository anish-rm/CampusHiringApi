namespace CampusHiring.Api.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Interviewer
{
    [Key]
    [ForeignKey(nameof(User))]
    public required string UserId { get; set; }

    public User? User { get; set; }

    public string Designation { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public IList<InterviewerAvailability> InterviewerAvailabilities { get; set; } = [];
    public IList<Interview> Interviews { get; set; } = [];
}
