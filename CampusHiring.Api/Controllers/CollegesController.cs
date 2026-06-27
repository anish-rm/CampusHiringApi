using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusHiring.Api.Domain;
using CampusHiring.Api.Application.Services;
using CampusHiring.Api.Application.DTOs.College;

namespace CampusHiring.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegesController(ICollegesService collegesService) : BaseApiController
    {
        // GET: api/Colleges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCollegesDto>>> GetColleges()
        {
            var result = await collegesService.GetCollegesAsync();
            return ToActionResult(result);
        }

        // GET: api/Colleges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCollegesDto>> GetCollege(int id)
        {
            var result = await collegesService.GetCollegeAsync(id);

            return ToActionResult(result);
        }

        // PUT: api/Colleges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollege(int id, UpdateCollegeDto college)
        {
            var result = await collegesService.UpdateCollegeAsync(id, college);

            return ToActionResult(result);
        }

        // POST: api/Colleges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetCollegesDto>> PostCollege(CreateCollegeDto college)
        {
            var result = await collegesService.CreateCollegeAsync(college);
            if (!result.IsSuccess)
            {
                return MapToErrors(result.Errors);
            }

            return CreatedAtAction("GetCollege", new { id = result.Value!.Id }, result.Value);
        }

        // DELETE: api/Colleges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollege(int id)
        {
            var result = await collegesService.DeleteCollegeAsync(id);
            return ToActionResult(result);
        }

    }
}
