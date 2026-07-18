namespace CampusHiring.Api.Application.DTOs.Interview;

public class GetInterviewRoundDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public int RoundNumber { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxScore { get; set; } = 100;
    public int PassingScore { get; set; } = 80;
}
