using System.ComponentModel.DataAnnotations;

namespace CampusHiring.Api.Application.DTOs.College;

public class CreateCollegeDto
{
    public required string Name { get; set; }
    public required string Location { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public long Phone { get; set; }
    public int NirfRank { get; set; }
}
