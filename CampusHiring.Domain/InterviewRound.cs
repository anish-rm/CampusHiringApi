namespace CampusHiring.Api.Domain;

public class InterviewRound
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public int RoundNumber { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxScore { get; set; } = 100;
    public int PassingScore { get; set; } = 80;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public IList<Interview> Interviews { get; set; } = [];
}
