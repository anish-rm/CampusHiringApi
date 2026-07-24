using CampusHiring.Api.Common.Enums;

namespace CampusHiring.Api.Application.DTOs.Interview;

public class GetCandidateSelectionDto
{
    public int Id { get; set; }
    public required string StudentUserId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public CandidateStatus CandidateStatus { get; set; }
    public DateTime ShortListedDate { get; set; }
    public string RejectionReason { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
