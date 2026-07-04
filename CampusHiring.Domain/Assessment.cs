namespace CampusHiring.Api.Domain;

public class Assessment
{
    public int Id { get; set; }
    public required string StudentUserId { get; set; }
    public Student? Student { get; set; }
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public int AssessmentTypeId { get; set; }
    public AssessmentType? AssessmentType { get; set; }
    public int Round { get; set; }
    public int Score { get; set; }
    public string Result { get; set; } = string.Empty;
    public DateTime AssessmentDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }

}
