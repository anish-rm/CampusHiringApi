using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusHiring.Api.Domain;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Company;
using CampusHiring.Api.Common.Results;
using Microsoft.AspNetCore.Authorization;
using CampusHiring.Api.Common.Constants;

namespace CampusHiring.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController(ICompaniesService companiesService) : BaseApiController
    {
        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCompanyDto>>> GetCompanies()
        {
            var result = await companiesService.GetCompaniesAsync();
            return ToActionResult(result);
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCompanyDto>> GetCompany(int id)
        {
            var result = await companiesService.GetCompanyAsync(id);
            return ToActionResult(result);
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = RoleNames.Admin)]
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
}
