using System.ComponentModel.DataAnnotations;

namespace CampusHiring.Api.Application.DTOs.Company;

public class UpdateCompanyDto : CreateCompanyDto
{
    public required int Id { get; set; }
}
