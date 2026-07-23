using AutoMapper;
using AutoMapper.QueryableExtensions;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Interview;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Results;
using CampusHiring.Api.Domain;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CampusHiring.Api.Application.Services;

public class InterviewsService(CampusHiringDbContext context, IMapper mapper, IAssessmentsService assessmentsService) : IInterviewsService
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

        if (round == null)
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
        if (id != roundDto.Id)
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

    public async Task<Result<IEnumerable<GetInterviewerAvailabilityDto>>> GetInterviewersAvailabilityAsync()
    {
        var availabilities = await context.InterviewerAvailabilities
            .AsNoTracking()
            .ProjectTo<GetInterviewerAvailabilityDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<IEnumerable<GetInterviewerAvailabilityDto>>.Success(availabilities);
    }

    public async Task<Result<GetInterviewerAvailabilityDto>> CreateInterviewerAvailabilityAsync(CreateInterviewerAvailabilityDto interviewerAvailabilityDto)
    {
        var company = await context.Companies.FindAsync(interviewerAvailabilityDto.CompanyId);
        if(company  == null)
        {
            return Result<GetInterviewerAvailabilityDto>.NotFound(new Error(ErrorCodes.NotFound, $"Company with id {interviewerAvailabilityDto.CompanyId} is not found"));
        }

        var interviewers = await context.Interviewers.FindAsync(interviewerAvailabilityDto.InterviewerUserId);
        if (interviewers == null)
        {
            return Result<GetInterviewerAvailabilityDto>.NotFound(new Error(ErrorCodes.NotFound, $"Interviewer with id {interviewerAvailabilityDto.InterviewerUserId} is not found"));
        }

        var interviewerAvailability = mapper.Map<InterviewerAvailability>(interviewerAvailabilityDto);
        context.InterviewerAvailabilities.Add(interviewerAvailability);
        await context.SaveChangesAsync();

        var result = mapper.Map<GetInterviewerAvailabilityDto>(interviewerAvailability);
        return Result<GetInterviewerAvailabilityDto>.Success(result);
    }


    public async Task<Result<IEnumerable<GetInterviewDto>>> GetInterviewsAsync()
    {
        var interviews = await context.Interviews
            .AsNoTracking()
            .ProjectTo<GetInterviewDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<IEnumerable<GetInterviewDto>>.Success(interviews);
    }

    public async Task<Result<GetInterviewDto>> GetInterviewAsync(int id)
    {
        var interview = await context.Interviews
            .AsNoTracking()
            .Where(ir => ir.Id == id)
            .ProjectTo<GetInterviewDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (interview == null)
        {
            return Result<GetInterviewDto>.NotFound(new Error(ErrorCodes.NotFound, $"Interview with id {id} is not found"));
        }

        return Result<GetInterviewDto>.Success(interview);

    }


    public async Task<Result> ScheduleInterviews(int companyId, int collegeId, DateTime interviewDate, int duration = 60, int roundNumber = 1)
    {
        var company = await context.Companies.FindAsync(companyId);
        if (company == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Company with id {companyId} is not found"));
        }

        var college = await context.Colleges.FindAsync(collegeId);
        if (college == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"college with id {collegeId} is not found"));
        }

        var interviewRound = await GetInterviewRound(companyId, roundNumber);
        if (interviewRound == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Round {roundNumber} for company {companyId} is not found"));
        }

        var studentIds = await FilterStudents(collegeId, companyId, roundNumber);

        if (studentIds == null || studentIds.Count == 0)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"No Students from give college {college.Name} is eligible for interviews"));
        }
        var interviews = new List<Interview>();
        var interviewers = await GetAvailableInterviewers(companyId, interviewDate, studentIds.Count);
        if (interviewers == null || interviewers.Count == 0)
            return Result.NotFound(new Error(ErrorCodes.NotFound, "No interviewers available for the requested date"));

        var assignCount = Math.Min(studentIds.Count, interviewers.Count);
        int assign = 0;
        for (assign = 0; assign < assignCount; assign++)
        {
            var slot = interviewers[assign];
            interviews.Add(new Interview
            {
                StudentUserId = studentIds[assign],
                InterviewerUserId = slot.InterviewerUserId,
                CompanyId = companyId,
                InterviewRoundId = interviewRound.Id,
                ScheduledStartTime = slot.StartTime,
                ScheduledEndTime = slot.EndTime,
            });
        }


        if (interviews.Count == 0)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"No Interviewers available for scheduling interviews"));
        }

        context.Interviews.AddRange(interviews);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    private async Task<List<InterviewerAvailability>> GetAvailableInterviewers(int companyId, DateTime interviewDate, int need)
    {
        if (need <= 0) return new List<InterviewerAvailability>();

        var slotStart = interviewDate;
        var slotEnd = interviewDate.AddDays(1);

        // keep transaction very short: select TOP(need) with UPDLOCK/READPAST, claim them, commit
        await using var tx = await context.Database.BeginTransactionAsync();

        var sql = FormattableStringFactory.Create(
            @"SELECT TOP({0}) *
          FROM InterviewerAvailabilities WITH (UPDLOCK, READPAST)
          WHERE CompanyId = {1} AND StartTime >= {2} AND EndTime < {3} AND IsAvailable = 1
          ORDER BY Id",
            need, companyId, slotStart, slotEnd);

        var interviewers = await context.InterviewerAvailabilities
            .FromSqlInterpolated(sql)
            .ToListAsync();

        if (interviewers.Count == 0)
        {
            await tx.CommitAsync();
            return interviewers;
        }

        var now = DateTime.UtcNow;
        foreach (var ia in interviewers)
        {
            ia.IsAvailable = false;
            ia.UpdatedAt = now;
        }

        // persist claimed state and release locks quickly
        await context.SaveChangesAsync();
        await tx.CommitAsync();

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

        var studentIds = await context.Students
            .Where(s => s.CollegeId == collegeId)
            .Select(s => s.UserId)
            .ToListAsync();

        if (studentIds.Count == 0)
        {
            return [];
        }

        int previousRound = roundNumber - 1;
        List<string> clearedStudentIds;
        if (previousRound == 0)
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
