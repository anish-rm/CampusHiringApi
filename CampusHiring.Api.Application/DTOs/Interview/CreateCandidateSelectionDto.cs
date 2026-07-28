using CampusHiring.Api.Common.Enums;

namespace CampusHiring.Api.Application.DTOs.Interview;

public class CreateCandidateSelectionDto
{
    public CandidateStatus CandidateStatus { get; set; } = CandidateStatus.Shortlisted;
    public DateTime ShortListedDate { get; set; } = DateTime.UtcNow;
    public string RejectionReason { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
