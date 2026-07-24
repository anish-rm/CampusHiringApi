namespace CampusHiring.Api.Application.DTOs.Interview;

public class UpdateInterviewDto
{
    public int Id { get; set; }
    public required string InterviewerUserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = string.Empty;
}
