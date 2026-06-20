using AutoMapper;
using AutoMapper.QueryableExtensions;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs;
using CampusHiring.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CampusHiring.Api.Application.Services;

public class AssessmentsService(CampusHiringDbContext context, IMapper mapper) : IAssessmentsService
{
    public async Task<IEnumerable<GetAssessmentsDto>> GetAssessmentsAsync()
    {
        var assessments = await context.Assessments
            .AsNoTracking()
            .ProjectTo<GetAssessmentsDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        return assessments;
    }

    public async Task<GetAssessmentDto?> GetAssessmentAsync(int id)
    {
        var assessment = await context.Assessments
            .Where(a => a.Id == id)
            .AsNoTracking()
            .ProjectTo<GetAssessmentDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return assessment;
    }

    public async Task UpdateAssessmentAsync(int id, UpdateAssessmentDto updateAssessmentDto)
    {
        var assessment = await context.Assessments.FindAsync(id);
        mapper.Map(updateAssessmentDto, assessment);
        //context.Entry(assessment).State = EntityState.Modified;
        context.Assessments.Update(assessment);
        await context.SaveChangesAsync();
    }

    public async Task<GetAssessmentDto> CreateAssessmentAsync(CreateAssessmentDto createAssessmentDto)
    {
        var assessment = mapper.Map<Assessment>(createAssessmentDto);
        context.Assessments.Add(assessment);
        await context.SaveChangesAsync();
        var assessmentDto = mapper.Map<GetAssessmentDto>(assessment);
        return assessmentDto;
    }

    public async Task DeleteAssessmentAsync(int id)
    {
        var assessment = await context.Assessments.FindAsync(id);
        context.Assessments.Remove(assessment);
        await context.SaveChangesAsync();
    }


    public async Task<bool> AssessmentExists(int id)
    {
        return await context.Assessments.AnyAsync(e => e.Id == id);
    }
}
