namespace CampusHiring.Api.Domain;

public class AssessmentType
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int MaxScore { get; set; }
    public int PassScore { get; set; }
    public int Duration { get; set; }
    public string Description { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public IEnumerable<Assessment> Assessments { get; set; } = [];
}
