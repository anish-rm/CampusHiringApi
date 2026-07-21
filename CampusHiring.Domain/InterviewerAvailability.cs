namespace CampusHiring.Api.Domain;

public class InterviewerAvailability
{
    public int Id { get; set; }
    public required string InterviewerUserId { get; set; }
    public Interviewer? Interviewer { get; set; }
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}
