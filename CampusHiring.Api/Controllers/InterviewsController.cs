using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Interview;
using Microsoft.AspNetCore.Mvc;

namespace CampusHiring.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
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


    [HttpPut("rounds/{id}")]
    public async Task<IActionResult> PutInterviewRound(int id, UpdateInterviewRoundDto roundDto)
    {
        var result = await interviewsService.UpdateInterviewRoundAsync(id, roundDto);

        return ToActionResult(result);
    }

    [HttpPost("rounds")]
    public async Task<ActionResult<GetInterviewRoundDto>> PostInterviewRound(CreateInterviewRoundDto roundDto)
    {
        var result = await interviewsService.CreateInterviewRoundAsync(roundDto);

        if(!result.IsSuccess)
        {
            return MapToErrors(result.Errors);
        }
        return CreatedAtAction("GetInterviewRound", new { id = result.Value!.Id }, result.Value);
    }

    [HttpDelete("rounds/{id}")]
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

    [HttpPost("availabilities")]
    public async Task<ActionResult<GetInterviewerAvailabilityDto>> PostInterviewAvailability(CreateInterviewerAvailabilityDto interviewerAvailabilityDto)
    {
        var result = await interviewsService.CreateInterviewerAvailabilityAsync(interviewerAvailabilityDto);

        if (!result.IsSuccess)
        {
            return MapToErrors(result.Errors);
        }
        return CreatedAtAction("GetInterviewRound", new { id = result.Value!.Id }, result.Value);
    }

    [HttpPost("schedule")]
    public async Task<ActionResult<GetInterviewerAvailabilityDto>> PostInterviewAvailability([FromQuery]int companyId, [FromQuery] int collegeId, [FromQuery] int batch, [FromQuery] DateTime interviewDate, [FromQuery] int duration = 60, [FromQuery] int roundNumber = 1)
    {
        var result = await interviewsService.ScheduleInterviews(companyId, collegeId, batch, interviewDate, duration, roundNumber);

        return ToActionResult(result);
    }

}
