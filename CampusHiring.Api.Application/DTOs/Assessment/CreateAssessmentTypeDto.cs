using System.ComponentModel.DataAnnotations;

namespace CampusHiring.Api.Application.DTOs.Assessment;

public class CreateAssessmentTypeDto
{
    public required string Name { get; set; }
    [Range(0, 100)]
    public int MaxScore { get; set; }
    [Range(0, 100)]
    public int PassScore { get; set; }
    public int Duration { get; set; }
    public string Description { get; set; } = string.Empty;
    public required int CompanyId { get; set; }
}
