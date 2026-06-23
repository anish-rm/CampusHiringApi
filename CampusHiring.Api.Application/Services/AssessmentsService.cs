using AutoMapper;
using AutoMapper.QueryableExtensions;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs;
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
        context.Assessments.Remove(assessment);
        await context.SaveChangesAsync();

        return Result.Success();
    }


    public async Task<bool> AssessmentExists(int id)
    {
        return await context.Assessments.AnyAsync(e => e.Id == id);
    }
}
