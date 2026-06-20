namespace CampusHiring.Api.Application.DTOs;

public class GetAssessmentsDto
{
    public int Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string AssessmentType { get; set; } = string.Empty;
    public int Score { get; set; }
    public string Result { get; set; } = string.Empty;
    public DateTime AssessmentDate { get; set; }
}
