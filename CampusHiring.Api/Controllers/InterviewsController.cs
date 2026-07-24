using Azure;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Interview;
using CampusHiring.Api.AuthorizationFilter;
using CampusHiring.Api.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CampusHiring.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class InterviewsController(IInterviewsService interviewsService) : BaseApiController
{

    // GET: api/Interviews
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetInterviewDto>>> GetInterviews()
    {
        var result = await interviewsService.GetInterviewsAsync();

        return ToActionResult(result);
    }

    // GET: api/Interviews/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetInterviewDto>> GetInterview(int id)
    {
        var result = await interviewsService.GetInterviewAsync(id);

        return ToActionResult(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> PutInterview(int id, UpdateInterviewDto interviewDto)
    {
        var result = await interviewsService.UpdateInterviewAsync(id, interviewDto);

        return ToActionResult(result);
    }

    [HttpPut("{id}/company/{companyId}")]
    [InterviewerOrSystemAdmin]
    public async Task<IActionResult> PutCompanyInterview(int id, int companyId, UpdateInterviewDto interviewDto)
    {
        var result = await interviewsService.UpdateInterviewAsync(id, interviewDto);

        return ToActionResult(result);
    }

    [HttpPatch("{id}/company/{companyId}")]
    [InterviewerOrSystemAdmin]
    public async Task<IActionResult> SubmitFeedback(int id, int companyId, JsonPatchDocument<PatchInterviewDto> patchDoc)
    {
        if(patchDoc == null)
        {
            return BadRequest("Patch document is required");
        }

        var result = await interviewsService.SubmitFeedbackAsync(id,companyId,patchDoc);

        return ToActionResult(result);
    }

    [HttpGet("rounds")]
    public async Task<ActionResult<IEnumerable<GetInterviewRoundDto>>> GetInterviewRounds()
    {
        var result = await interviewsService.GetInterviewRoundsAsync();

        return ToActionResult(result);
    }

    [HttpGet("rounds/{id}")]
    public async Task<ActionResult<GetInterviewRoundDto>> GetInterviewRound(int id)
    {
        var result = await interviewsService.GetInterviewRoundAsync(id);

        return ToActionResult(result);
    }

    [HttpGet("rounds/company/{companyId}")]
    public async Task<ActionResult<IEnumerable<GetInterviewRoundDto>>> GetInterviewRounds(int companyId)
    {
        var result = await interviewsService.GetCompanyInterviewRoundsAsync(companyId);

        return ToActionResult(result);
    }


    [HttpPut("company/{companyId}/rounds/{id}")]
    [InterviewerOrSystemAdmin]
    public async Task<IActionResult> PutInterviewRound(int id, int companyId, UpdateInterviewRoundDto roundDto)
    {
        var result = await interviewsService.UpdateInterviewRoundAsync(id, companyId, roundDto);

        return ToActionResult(result);
    }

    [HttpPost("company/{companyId}/rounds")]
    [InterviewerOrSystemAdmin]
    public async Task<ActionResult<GetInterviewRoundDto>> PostInterviewRound(int companyId, CreateInterviewRoundDto roundDto)
    {
        var result = await interviewsService.CreateInterviewRoundAsync(companyId, roundDto);

        if(!result.IsSuccess)
        {
            return MapToErrors(result.Errors);
        }
        return CreatedAtAction("GetInterviewRound", new { id = result.Value!.Id }, result.Value);
    }

    [HttpDelete("rounds/{id}")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> DeleteInterviewRound(int id)
    {
        var result = await interviewsService.DeleteInterviewRoundAsync(id);
        return ToActionResult(result);
    }

    [HttpGet("availabilities")]
    public async Task<ActionResult<IEnumerable<GetInterviewerAvailabilityDto>>> GetInterviewAvailabilities()
    {
        var result = await interviewsService.GetInterviewersAvailabilityAsync();

        return ToActionResult(result);
    }

    [HttpPost("company/{companyId}/availabilities")]
    [InterviewerOrSystemAdmin]
    public async Task<ActionResult<GetInterviewerAvailabilityDto>> PostInterviewAvailability(int companyId, CreateInterviewerAvailabilityDto interviewerAvailabilityDto)
    {
        var result = await interviewsService.CreateInterviewerAvailabilityAsync(companyId, interviewerAvailabilityDto);

        if (!result.IsSuccess)
        {
            return MapToErrors(result.Errors);
        }
        return CreatedAtAction("GetInterviewRound", new { id = result.Value!.Id }, result.Value);
    }

    [HttpPost("schedule/company/{companyId}/college/{collegeId}")]
    [InterviewerOrSystemAdmin]
    public async Task<ActionResult<GetInterviewerAvailabilityDto>> PostInterviewAvailability([FromRoute]int companyId, [FromRoute] int collegeId, [FromQuery] int batch, [FromQuery] DateTime interviewDate, [FromQuery] int duration = 60, [FromQuery] int roundNumber = 1)
    {
        var result = await interviewsService.ScheduleInterviews(companyId, collegeId, batch, interviewDate, duration, roundNumber);

        return ToActionResult(result);
    }

    [HttpGet("candidateStatus")]
    public async Task<ActionResult<IEnumerable<GetCandidateSelectionDto>>> GetCandidateStatus()
    {
        var result = await interviewsService.GetCandidateSelectionsAsync();

        return ToActionResult(result);
    }

    [HttpGet("candidateStatus/college/{collegeId}")]
    public async Task<ActionResult<IEnumerable<GetCandidateSelectionDto>>> GetCollegeCandidateStatus(int collegeId)
    {
        var result = await interviewsService.GetCollegeCandidateSelectionsAsync(collegeId);

        return ToActionResult(result);
    }

}
