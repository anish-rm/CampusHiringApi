using CampusHiring.Api.Domain;

namespace CampusHiring.Api.Application.DTOs.Interview;

public class CreateInterviewerAvailabilityDto
{
    public required string InterviewerUserId { get; set; }
    public required int CompanyId { get; set; }
    public bool IsAvailable { get; set; } = true;
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
}
