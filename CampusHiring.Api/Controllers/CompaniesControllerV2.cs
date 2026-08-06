using Asp.Versioning;
using CampusHiring.Api.Application.DTOs.Company;
using Microsoft.AspNetCore.Mvc;

namespace CampusHiring.Api.Controllers;

[Route("api/v{version:apiVersion}/companies")]
[ApiController]
[ApiVersion("2.0", Deprecated = true)]
public class CompaniesControllerV2 : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCompanyDto>>> GetCompanies()
    {
        return Ok("Not implemented yet");
    }
}
