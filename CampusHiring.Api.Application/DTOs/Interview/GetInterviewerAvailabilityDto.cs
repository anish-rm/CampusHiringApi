namespace CampusHiring.Api.Application.DTOs.Interview;

public class GetInterviewerAvailabilityDto
{
    public int Id { get; set; }
    public required string InterviewerUserId { get; set; }
    public string InterviewerName { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
