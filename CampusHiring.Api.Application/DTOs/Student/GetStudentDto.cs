using CampusHiring.Api.Common.Enums;
using CampusHiring.Api.Domain;

namespace CampusHiring.Api.Application.DTOs.Student;

public class GetStudentDto
{
    public required string UserId { get; set; }
    public int Cgpa { get; set; }
    public int Batch { get; set; }
    public Department Department { get; set; }
    public string ResumeUrl { get; set; } = string.Empty;
    public string CollegeName { get; set; } = string.Empty;
}
