using AutoMapper;
using AutoMapper.QueryableExtensions;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Interview;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Results;
using CampusHiring.Api.Domain;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace CampusHiring.Api.Application.Services;

public class InterviewsService(CampusHiringDbContext context, IMapper mapper, IAssessmentsService assessmentsService)
{
    public async Task<Result<IEnumerable<GetInterviewRoundDto>>> GetInterviewRoundsAsync()
    {
        var rounds = await context.InterviewRounds
            .AsNoTracking()
            .ProjectTo<GetInterviewRoundDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<IEnumerable<GetInterviewRoundDto>>.Success(rounds);

    }

    public async Task<Result<GetInterviewRoundDto>> GetInterviewRoundAsync(int id)
    {
        var round = await context.InterviewRounds
            .AsNoTracking()
            .Where(ir => ir.Id == id)
            .ProjectTo<GetInterviewRoundDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if(round == null)
        {
            return Result<GetInterviewRoundDto>.NotFound(new Error(ErrorCodes.NotFound, $"Interview rounds with id {id} is not found"));
        }

        return Result<GetInterviewRoundDto>.Success(round);

    }

    public async Task<Result<IEnumerable<GetInterviewRoundDto>>> GetCompanyInterviewRoundsAsync(int id)
    {
        var rounds = await context.InterviewRounds
            .AsNoTracking()
            .Where(ir => ir.CompanyId == id)
            .ProjectTo<GetInterviewRoundDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        if (rounds.Count == 0)
        {
            return Result<IEnumerable<GetInterviewRoundDto>>.NotFound(new Error(ErrorCodes.NotFound, $"Interview rounds with college id {id} is not found"));
        }

        return Result<IEnumerable<GetInterviewRoundDto>>.Success(rounds);

    }

    public async Task<Result> UpdateInterviewRoundAsync(int id, UpdateInterviewRoundDto roundDto)
    {
        if(id != roundDto.Id)
        {
            return Result.BadRequest(new Error(ErrorCodes.BadRequest, $"Id {id} does not match with id {roundDto.Id} passed in body"));
        }
        var round = await context.InterviewRounds.FindAsync(id);

        if (round == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Interview rounds with id {id} is not found"));
        }

        var company = await context.Companies.FindAsync(roundDto.CompanyId);
        if (company == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Company with id {roundDto.CompanyId} is not found"));
        }

        mapper.Map(roundDto, round);
        round.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success();

    }

    public async Task<Result<GetInterviewRoundDto>> CreateInterviewRoundAsync(CreateInterviewRoundDto roundDto)
    {
        var company = await context.Companies.FindAsync(roundDto.CompanyId);
        if (company == null)
        {
            return Result<GetInterviewRoundDto>.NotFound(new Error(ErrorCodes.NotFound, $"Company with id {roundDto.CompanyId} is not found"));
        }

        var round = mapper.Map<InterviewRound>(roundDto);
        context.InterviewRounds.Add(round);
        await context.SaveChangesAsync();
        var result = mapper.Map<GetInterviewRoundDto>(roundDto);
        return Result<GetInterviewRoundDto>.Success(result);

    }

    public async Task<Result> DeleteInterviewRoundAsync(int id)
    {
        var round = await context.InterviewRounds.FindAsync(id);

        if (round == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Interview rounds with id {id} is not found"));
        }
        context.InterviewRounds.Remove(round);
        await context.SaveChangesAsync();
        return Result.Success();

    }

    //public async Task<Result> ScheduleInterviews(int companyId, int collegeId, DateTime interviewDate, int duration = 60, int roundNumber = 1)
    //{
    //    var company = await context.Companies.FindAsync(companyId);
    //    if(company == null)
    //    {
    //        return Result.NotFound(new Error(ErrorCodes.NotFound, $"Company with id {companyId} is not found"));
    //    }

    //    var college = await context.Colleges.FindAsync(collegeId);
    //    if (college == null)
    //    {
    //        return Result.NotFound(new Error(ErrorCodes.NotFound, $"college with id {collegeId} is not found"));
    //    }

    //    var interviewRound = await GetInterviewRound(companyId, roundNumber);
    //    if (interviewRound == null)
    //    {
    //        return Result.NotFound(new Error(ErrorCodes.NotFound, $"Round {roundNumber} for company {companyId} is not found"));
    //    }

    //    var studentIds = await FilterStudents(collegeId, companyId, roundNumber);

    //    if(studentIds == null || studentIds.Count == 0)
    //    {
    //        return Result.NotFound(new Error(ErrorCodes.NotFound, $"No Students from give college {college.Name} is eligible for interviews"));
    //    }
    //    var startdatetime = interviewDate;
    //    var interviews = new List<Interview>();
    //    var interviewers = await GetAvailableInterviewers(companyId, startdatetime, duration);
    //    int studentsassigned = 0;
    //    while(interviewers != null && studentIds.Count != studentsassigned)
    //    {
    //        for(int i = 0; i < interviewers.Count; i++)
    //        {
    //            var interview = new Interview
    //            {
    //                StudentUserId = studentIds[studentsassigned],
    //                InterviewerUserId = interviewers[i].InterviewerUserId,
    //                CompanyId = companyId,
    //                InterviewRoundId = interviewRound.Id,
    //                ScheduledStartTime = startdatetime,
    //                ScheduledEndTime = startdatetime.AddMinutes(duration),
    //            };
    //            interviews.Add(interview);
    //        }
    //        interviewers = await GetAvailableInterviewers(companyId, startdatetime, duration);
    //    }

    //    if (interviewers == null)
    //    {
    //        if (interviews.Count == 0)
    //        {
    //            return Result.BadRequest(new Error(ErrorCodes.NotFound, $"No Interviewers available"));
    //        }
    //        //we have assigned some interviews based on available interviewers
    //    }

    //    var interviews = studentIds
    //        .Select(sid => new Interview
    //        {
    //            StudentUserId = sid,

    //        })
        
    //}

    private async Task<List<InterviewerAvailability>> GetAvailableInterviewers(int companyId, DateTime interviewDate, int duration)
    {
        var slotStart = interviewDate;
        var slotEnd = interviewDate.AddMinutes(duration);

        var interviewers = await context.InterviewerAvailabilities
            .Where(ia => ia.CompanyId == companyId
            && ia.StartTime < slotEnd
            && ia.EndTime > slotStart
            && ia.IsAvailable == true)
            .ToListAsync();

        return interviewers;
    }


    private async Task<InterviewRound?> GetInterviewRound(int companyId, int roundNumber = 1)
    {
        var round = await context.InterviewRounds
            .AsNoTracking()
            .Where(ir => ir.CompanyId == companyId && ir.RoundNumber == roundNumber)
            .FirstOrDefaultAsync();

        return round;
    }

    private async Task<List<string>> GetInterviewClearedStudentIds(List<string> studentIds, int companyId, int round)
    {
        var result = await context.Interviews
            .Where(i => i.CompanyId == companyId
            && i.InterviewRound!.RoundNumber == round
            && studentIds.Contains(i.StudentUserId)
            && i.Recommendation == "Pass")
            .Select(i => i.StudentUserId)
            .Distinct()
            .ToListAsync();

        return result;
    }

    private async Task<List<string>> FilterStudents(int collegeId, int companyId, int roundNumber = 1)
    {

        var studentIds =  await context.Students
            .Where(s => s.CollegeId == collegeId)
            .Select(s => s.UserId)
            .ToListAsync();

        if(studentIds.Count == 0)
        {
            return [];
        }

        int previousRound = roundNumber - 1;
        List<string> clearedStudentIds;
        if(previousRound == 0)
        {
            int finalAssessmentRound = await assessmentsService.GetLastAssessmentRoundAsync(studentIds, companyId);
            clearedStudentIds = await assessmentsService.GetAssessmentClearedStudentIds(studentIds, finalAssessmentRound, companyId);
        }
        else
        {
            clearedStudentIds = await GetInterviewClearedStudentIds(studentIds, companyId, previousRound);
        }
        return clearedStudentIds;
    }

}
