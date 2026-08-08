using Asp.Versioning;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Company;
using CampusHiring.Api.Application.DTOs.Interview;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CampusHiring.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class CompaniesController(ICompaniesService companiesService) : BaseApiController
{
    /// <summary>
    /// Returns a list of companies
    /// </summary>
    /// <returns>An asynchronous operation that returns an <see cref="ActionResult{T}"/> containing a collection of <see cref="GetCompanyDto"/> objects representing the companies</returns>
    // GET: api/Companies
    [HttpGet]
    //[OutputCache]
    [OutputCache(PolicyName = CacheConstants.AuthenticatedUserCachingPolicy)]
    public async Task<ActionResult<IEnumerable<GetCompanyDto>>> GetCompanies()
    {
        var result = await companiesService.GetCompaniesAsync();
        return ToActionResult(result);
    }

    /// <summary>
    /// Returns the details of company with specified parameter
    /// </summary>
    /// <response code="200"></response>
    /// <response code="404">Company not found</response>
    /// <returns>An asynchronous operation that returns an <see cref="ActionResult{T}"/> containing a collection of <see cref="GetCompanyDto"/> objects representing the companies</returns>
    // GET: api/Companies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCompanyDto>> GetCompany(int id)
    {
        var result = await companiesService.GetCompanyAsync(id);
        return ToActionResult(result);
    }

    [HttpGet("{id}/interviewers")]
    public async Task<ActionResult<IEnumerable<GetInterviewerDto>>> GetCompanyInterviewers(int id)
    {
        var result = await companiesService.GetInterviewersAsync(id);
        return ToActionResult(result);
    }

    // PUT: api/Companies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = RoleNames.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutCompany(int id, UpdateCompanyDto company)
    {
        var result = await companiesService.UpdateCompanyAsync(id, company);

        return ToActionResult(result);
    }

    // POST: api/Companies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<ActionResult<Company>> PostCompany(CreateCompanyDto company)
    {
        var result = await companiesService.CreateCompanyAsync(company);
        if (!result.IsSuccess)
        {
            return MapToErrors(result.Errors);
        }
        return CreatedAtAction("GetCompany", new { id = result.Value!.Id }, result.Value);
    }

    // DELETE: api/Companies/5
    [HttpDelete("{id}")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var result = await companiesService.DeleteCompanyAsync(id);

        return ToActionResult(result);
    }

}
