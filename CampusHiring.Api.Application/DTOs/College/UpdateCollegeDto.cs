namespace CampusHiring.Api.Application.DTOs.College;

public class UpdateCollegeDto
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public long Phone { get; set; }
    public int NirfRank { get; set; }
}
