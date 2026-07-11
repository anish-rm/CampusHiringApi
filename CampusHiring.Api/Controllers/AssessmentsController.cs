using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Assessment;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Enums;
using CampusHiring.Api.Common.Models.Filtering;
using CampusHiring.Api.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusHiring.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("college/{collegeId}")]
        public async Task<ActionResult<IEnumerable<GetAssessmentDto>>> GetCollegeAssessment(int collegeId, [FromQuery]AssessmentFilterParameter filter)
        {
            var assessment = await assessmentsService.GetCollegeAssessmentsAsync(collegeId,filter);

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

            return CreatedAtAction("GetAssessments", new { id = assessment.Value!.Id }, assessment.Value);
        }

        // DELETE: api/Assessments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessment(int id)
        {
            var result = await assessmentsService.DeleteAssessmentAsync(id);

            return ToActionResult(result);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<GetAssessmentTypeDto>>> GetAssessementTypes()
        {
            var result = await assessmentsService.GetAssessmentTypesAsync();
            return ToActionResult(result);
        }

        [HttpGet("types/{id}")]
        public async Task<ActionResult<GetAssessmentTypeDto>> GetAssessementType(int id)
        {
            var result = await assessmentsService.GetAssessmentTypeAsync(id);
            return ToActionResult(result);
        }

        [HttpPut("types/{id}")]
        public async Task<ActionResult> UpdateAssessementType(int id,UpdateAssessmentTypeDto assessmentTypeDto)
        {
            var result = await assessmentsService.UpdateAssessmentTypeAsync(id,assessmentTypeDto);
            return ToActionResult(result);
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost("types")]
        public async Task<ActionResult<GetAssessmentTypeDto>> PostAssessmentType(CreateAssessmentTypeDto createAssessmentTypeDto)
        {
            var result = await assessmentsService.CreateAssessmentTypeAsync(createAssessmentTypeDto);
            if (!result.IsSuccess)
            {
                return MapToErrors(result.Errors);
            }
            return CreatedAtAction("GetAssessment", new { id = result.Value!.Id }, result.Value);
        }

        [HttpDelete("types/{id}")]
        public async Task<ActionResult> DeleteAssessementType(int id)
        {
            var result = await assessmentsService.DeleteAssessmentTypeAsync(id);
            return ToActionResult(result);
        }

        [HttpPost("assign/{collegeId:int}")]
        public async Task<ActionResult> AssignAssessments([FromRoute]int collegeId, [FromQuery]AssignAssessmentFilterParameter filter)
        {
            var result = await assessmentsService.AssignAssessments(collegeId,filter);
        
            return ToActionResult(result);
        }

    }
}
