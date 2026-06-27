using System.ComponentModel.DataAnnotations;

namespace CampusHiring.Api.Application.DTOs.Company;

public class CreateCompanyDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Location { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public long Phone { get; set; }
}
