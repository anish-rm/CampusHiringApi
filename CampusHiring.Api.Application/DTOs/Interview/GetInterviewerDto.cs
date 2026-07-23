namespace CampusHiring.Api.Application.DTOs.Interview;

public class GetInterviewerDto
{
    public required string UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
}
