using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Assessment;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusHiring.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentsController(IAssessmentsService assessmentsService) : BaseApiController
    {

        // GET: api/Assessments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAssessmentsDto>>> GetAssessments()
        {
            var assessments = await assessmentsService.GetAssessmentsAsync();
            return ToActionResult(assessments);
        }

        // GET: api/Assessments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAssessmentDto?>> GetAssessment(int id)
        {
            var assessment = await assessmentsService.GetAssessmentAsync(id);

            return ToActionResult(assessment);
        }

        // PUT: api/Assessments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssessment(int id, UpdateAssessmentDto updateAssessmentDto)
        {
            
            var result = await assessmentsService.UpdateAssessmentAsync(id, updateAssessmentDto);
           

            return ToActionResult(result);
        }

        // POST: api/Assessments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Assessment>> PostAssessment(CreateAssessmentDto createAssessmentDto)
        {
            var assessment = await assessmentsService.CreateAssessmentAsync(createAssessmentDto);

            if (!assessment.IsSuccess)
            {
                return MapToErrors(assessment.Errors);
            }

            return CreatedAtAction("GetAssessment", new { id = assessment.Value!.Id }, assessment.Value);
        }

        // DELETE: api/Assessments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessment(int id)
        {
            var result = await assessmentsService.DeleteAssessmentAsync(id);

            return ToActionResult(result);
        }

    }
}
