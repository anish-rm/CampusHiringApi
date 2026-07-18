namespace CampusHiring.Api.Application.DTOs.Interview;

public class CreateInterviewRoundDto
{
    public required int CompanyId { get; set; }
    public required int RoundNumber { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxScore { get; set; }
    public int PassingScore { get; set; }
}
