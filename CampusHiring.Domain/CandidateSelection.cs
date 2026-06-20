using CampusHiring.Api.Common.Enums;

namespace CampusHiring.Api.Domain;

public class CandidateSelection
{
    public int Id { get; set; }
    public required string StudentUserId { get; set; }
    public Student? Student { get; set; }
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public CandidateStatus CandidateStatus { get; set; } = CandidateStatus.Shortlisted;
    public DateTime ShortListedDate { get; set; } = DateTime.UtcNow;
    public string RejectionReason { get; set; } = string.Empty;
    public string Notes {  get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}
