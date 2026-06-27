using CampusHiring.Api.Common.Enums;

namespace CampusHiring.Api.Application.DTOs.Assessment;

public class GetAssessmentDto
{
    public int Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int Cgpa { get; set; }
    public Department Department { get; set; }
    public string CollegeName {  get; set; } = string.Empty;
    public string CompanyName {  get; set; } = string.Empty;
    public string AssessmentType {  get; set; } = string.Empty;
    public int MaxScore { get; set; }
    public int PassScore { get; set; }
    public int Score { get; set; }
    public string Result { get; set; } = string.Empty;
    public DateTime AssessmentDate { get; set; }
}
