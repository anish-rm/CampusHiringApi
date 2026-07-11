using System.ComponentModel.DataAnnotations;

namespace CampusHiring.Api.Application.DTOs.Assessment;

public class UpdateAssessmentDto : CreateAssessmentDto
{
    [Required]
    public int Id { get; set; }
    public int Score { get; set; }
}
