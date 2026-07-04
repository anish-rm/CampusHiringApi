using AutoMapper;
using AutoMapper.QueryableExtensions;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Assessment;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Results;
using CampusHiring.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CampusHiring.Api.Application.Services;

public class AssessmentsService(CampusHiringDbContext context, IMapper mapper) : IAssessmentsService
{
    public async Task<Result<IEnumerable<GetAssessmentsDto>>> GetAssessmentsAsync()
    {
        var assessments = await context.Assessments
            .AsNoTracking()
            .ProjectTo<GetAssessmentsDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        return Result<IEnumerable<GetAssessmentsDto>>.Success(assessments);
    }

    public async Task<Result<GetAssessmentDto?>> GetAssessmentAsync(int id)
    {
        var assessment = await context.Assessments
            .Where(a => a.Id == id)
            .AsNoTracking()
            .ProjectTo<GetAssessmentDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (assessment == null)
        {
            return Result<GetAssessmentDto?>.NotFound(new Error(ErrorCodes.NotFound,$"Assessment with id {id} not found"));
        }

        return Result<GetAssessmentDto?>.Success(assessment);
    }

    public async Task<Result> UpdateAssessmentAsync(int id, UpdateAssessmentDto updateAssessmentDto)
    {
        if (id != updateAssessmentDto.Id)
        {
            return Result.BadRequest(new Error(ErrorCodes.BadRequest, $"Id {id} is not matching with provided id {updateAssessmentDto.Id}"));
        }
        var assessment = await context.Assessments.FindAsync(id);
        if(assessment == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Assessment with id {id} not found"));
        }

        var company = await context.Companies.FindAsync(updateAssessmentDto.CompanyId);
        if (company == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Company with id {updateAssessmentDto.CompanyId} not found"));
        }

        var assessmentType = await context.AssessmentTypes.FindAsync(updateAssessmentDto.AssessmentTypeId);
        if (assessmentType == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"AssessmentType with id {updateAssessmentDto.AssessmentTypeId} not found"));
        }

        mapper.Map(updateAssessmentDto, assessment);
        //context.Entry(assessment).State = EntityState.Modified;
        context.Assessments.Update(assessment);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<GetAssessmentDto>> CreateAssessmentAsync(CreateAssessmentDto createAssessmentDto)
    {
        var assessment = mapper.Map<Assessment>(createAssessmentDto);
        context.Assessments.Add(assessment);
        await context.SaveChangesAsync();
        var assessmentDto = mapper.Map<GetAssessmentDto>(assessment);
        return Result<GetAssessmentDto>.Success(assessmentDto);
    }

    public async Task<Result> DeleteAssessmentAsync(int id)
    {
        var assessment = await context.Assessments.FindAsync(id);
        if (assessment == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Assessment with id {id} not found"));
        }
        context.Assessments.Remove(assessment);
        await context.SaveChangesAsync();

        return Result.Success();
    }

    //Assesment Type
    public async Task<Result<IEnumerable<GetAssessmentTypeDto>>> GetAssessmentTypesAsync()
    {
        var assessmentTypes = await context.AssessmentTypes
                        .AsNoTracking()
                        .ProjectTo<GetAssessmentTypeDto>(mapper.ConfigurationProvider)
                        .ToListAsync();

        return Result<IEnumerable<GetAssessmentTypeDto>>.Success(assessmentTypes);
    }

    public async Task<Result<GetAssessmentTypeDto>> GetAssessmentTypeAsync(int id)
    {
        var assessmentType = await context.AssessmentTypes
            .AsNoTracking()
            .Where(a => a.Id == id)
            .ProjectTo<GetAssessmentTypeDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (assessmentType == null)
        {
            return Result<GetAssessmentTypeDto>.NotFound(new Error(ErrorCodes.NotFound, $"Assessment Type with id {id} not found"));
        }

        return Result<GetAssessmentTypeDto>.Success(assessmentType);
    }

    public async Task<Result> UpdateAssessmentTypeAsync(int id,UpdateAssessmentTypeDto assessmentTypeDto)
    {
        if(id != assessmentTypeDto.Id)
        {
            return Result.BadRequest(new Error(ErrorCodes.BadRequest, $"Id {id} is not matching with provided id {assessmentTypeDto.Id}"));
        }
        var assessmentType = await context.AssessmentTypes.FindAsync(id);

        if (assessmentType == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Assessment Type with id {id} not found"));
        }

        mapper.Map(assessmentTypeDto, assessmentType);
        await context.SaveChangesAsync();
        return Result.Success();
    }
    public async Task<Result<GetAssessmentTypeDto>> CreateAssessmentTypeAsync(CreateAssessmentTypeDto assessmentTypeDto)
    {
        var company = await context.Companies.FindAsync(assessmentTypeDto.CompanyId);
        if(company == null)
        {
            return Result<GetAssessmentTypeDto>.NotFound(new Error(ErrorCodes.NotFound, $"Company with id {assessmentTypeDto.CompanyId} not found"));
        }

        var assessmentType = mapper.Map<AssessmentType>(assessmentTypeDto);
        context.AssessmentTypes.Add(assessmentType);
        await context.SaveChangesAsync();
        var resultDto = mapper.Map<GetAssessmentTypeDto>(assessmentType);
        return Result<GetAssessmentTypeDto>.Success(resultDto);
    }
    public async Task<Result> DeleteAssessmentTypeAsync(int id)
    {
        var assessmentType = await context.AssessmentTypes.FindAsync(id);

        if (assessmentType == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Assessment Type with id {id} not found"));
        }
        context.AssessmentTypes.Remove(assessmentType);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AssignAssessments(int collegeId, int assessmentTypeId, int round = 1)
    {
        var assessmentType = await context.AssessmentTypes.FindAsync(assessmentTypeId);
        if (assessmentType == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Assessment Type with id {assessmentTypeId} not found"));
        }

        var studentsIds = await context.Students
            .Where(s => s.CollegeId == collegeId)
            .Select(s => s.UserId)
            .ToListAsync();

        if(studentsIds.Count == 0)
        {
            var college = await context.Colleges.FindAsync(collegeId);
            if (college == null)
            {
                return Result.NotFound(new Error(ErrorCodes.NotFound, $"College with id {collegeId} not found"));
            }
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"No students in the given college"));
        }

        var assignedStudentIds = await context.Assessments
            .Where(a => a.AssessmentTypeId == assessmentTypeId && studentsIds.Contains(a.StudentUserId))
            .Select(a => a.StudentUserId)
            .ToListAsync();


        int previousRound = round - 1;
        var clearedStudentsIds = round > 1 ? await context.Assessments
            .Where(a => studentsIds.Contains(a.StudentUserId) 
                        && a.Round == previousRound 
                        && a.Result == "Pass")
            .Select(a => a.StudentUserId)
            .ToListAsync() : studentsIds;
       

        var newAssessments = clearedStudentsIds
            .Where(sid => !assignedStudentIds.Contains(sid))
            .Select(sid => new Assessment
            {
                StudentUserId = sid,
                CompanyId = assessmentType.CompanyId,
                AssessmentTypeId = assessmentTypeId,
                AssessmentDate = DateTime.UtcNow,
                Round = round
            })
            .ToList();

        if(newAssessments.Count == 0)
        {
            return Result.BadRequest(new Error(ErrorCodes.BadRequest, $"Students are already assigned with given assessment Type"));
        }


        context.Assessments.AddRange(newAssessments);
        await context.SaveChangesAsync();

        return Result.Success();

    }

    public async Task<bool> AssessmentExists(int id)
    {
        return await context.Assessments.AnyAsync(e => e.Id == id);
    }
}
