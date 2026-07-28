using CampusHiring.Api.Common.Enums;

namespace CampusHiring.Api.Application.DTOs.Interview;

public class UpdateCandidateSelectionDto
{
    public CandidateStatus CandidateStatus { get; set; } = CandidateStatus.Shortlisted;
    public string RejectionReason { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string Feedback { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
}
