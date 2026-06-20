using System.ComponentModel.DataAnnotations;

namespace CampusHiring.Api.Application.DTOs;

public class CreateAssessmentDto
{
    public required string StudentUserId { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public int AssessmentTypeId { get; set; }
    public DateTime AssessmentDate { get; set; }
}
