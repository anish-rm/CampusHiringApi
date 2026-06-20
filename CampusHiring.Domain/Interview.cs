using CampusHiring.Api.Common.Enums;

namespace CampusHiring.Api.Domain;

public class Interview
{
    public int Id { get; set; }
    public required string StudentUserId { get; set; }
    public Student? Student { get; set; }
    public required string InterviewerUserId { get; set; }
    public Interviewer? Interviewer { get; set; }
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public int InterviewRoundId { get; set; }
    public InterviewRound? InterviewRound { get; set; }
    public DateTime ScheduledStartTime { get; set; }
    public DateTime ScheduledEndTime { get; set; }
    public DateTime StartTime {  get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public InterviewStatus InterviewStatus { get; set; } = InterviewStatus.Scheduled;
    public string Feedback { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public DateTime FeedbackSubmissionDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }

}
