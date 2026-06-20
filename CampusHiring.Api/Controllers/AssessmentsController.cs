using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusHiring.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentsController(IAssessmentsService assessmentsService) : ControllerBase
    {

        // GET: api/Assessments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAssessmentsDto>>> GetAssessments()
        {
            var assessments = await assessmentsService.GetAssessmentsAsync();
            return Ok(assessments);
        }

        // GET: api/Assessments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAssessmentDto>> GetAssessment(int id)
        {
            var assessment = await assessmentsService.GetAssessmentAsync(id);

            if (assessment == null)
            {
                return NotFound();
            }

            return Ok(assessment);
        }

        // PUT: api/Assessments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssessment(int id, UpdateAssessmentDto updateAssessmentDto)
        {
            if (id != updateAssessmentDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await assessmentsService.UpdateAssessmentAsync(id, updateAssessmentDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await assessmentsService.AssessmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Assessments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Assessment>> PostAssessment(CreateAssessmentDto createAssessmentDto)
        {
            var assessment = await assessmentsService.CreateAssessmentAsync(createAssessmentDto);

            return CreatedAtAction("GetAssessment", new { id = assessment.Id }, assessment);
        }

        // DELETE: api/Assessments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessment(int id)
        {
            await assessmentsService.DeleteAssessmentAsync(id);

            return NoContent();
        }

    }
}
