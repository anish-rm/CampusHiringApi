using System.ComponentModel.DataAnnotations;

namespace CampusHiring.Api.Application.DTOs;

public class UpdateAssessmentDto : CreateAssessmentDto
{
    [Required]
    public int Id { get; set; }
}
