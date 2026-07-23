using CampusHiring.Api.Common.Enums;
using CampusHiring.Api.Domain;

namespace CampusHiring.Api.Application.DTOs.Interview;

public class GetInterviewDto
{
    public int Id { get; set; }
    public string StudentUserId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public required string InterviewerUserId { get; set; }
    public string InterviewerName { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public int InterviewRoundId { get; set; }
    public int RoundNumber { get; set; }
    public DateTime ScheduledStartTime { get; set; }
    public DateTime ScheduledEndTime { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public InterviewStatus InterviewStatus { get; set; } = InterviewStatus.Scheduled;
    public string Feedback { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public DateTime FeedbackSubmissionDate { get; set; }
}
