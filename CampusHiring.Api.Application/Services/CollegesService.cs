using AutoMapper;
using AutoMapper.QueryableExtensions;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.College;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Results;
using CampusHiring.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CampusHiring.Api.Application.Services;

public class CollegesService(CampusHiringDbContext context, IMapper mapper) : ICollegesService
{
    public async Task<Result<IEnumerable<GetCollegesDto>>> GetCollegesAsync()
    {
        var colleges = await context.Colleges
            .AsNoTracking()
            .ProjectTo<GetCollegesDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<IEnumerable<GetCollegesDto>>.Success(colleges);
    }

    public async Task<Result<GetCollegesDto>> GetCollegeAsync(int id)
    {

        var college = await context.Colleges
            .Where(c => c.Id == id)
            .ProjectTo<GetCollegesDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (college == null)
        {
            return Result<GetCollegesDto>.NotFound(new Error(ErrorCodes.NotFound, $"College with id {id} is not found"));
        }

        return Result<GetCollegesDto>.Success(college);
    }

    public async Task<Result> UpdateCollegeAsync(int id, UpdateCollegeDto collegeDto)
    {
        var college = await context.Colleges.FindAsync(id);
        if (college == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"College with id {id} is not found"));
        }
        var isCollegeWithSameName = await context.Colleges.FirstOrDefaultAsync(c => c.Id != college.Id && c.Name == collegeDto.Name);
        if (isCollegeWithSameName != null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"College with name {collegeDto.Name} already exists"));
        }
        var isCollegeWithSameEmail = await context.Colleges.FirstOrDefaultAsync(c => c.Id != college.Id && c.Email == collegeDto.Email);
        if (isCollegeWithSameEmail != null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"College with email {collegeDto.Email} already exists"));
        }
        //sening results
        mapper.Map(collegeDto, college);
        college.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<GetCollegesDto>> CreateCollegeAsync(CreateCollegeDto collegeDto)
    {
        var college = mapper.Map<College>(collegeDto);
        context.Colleges.Add(college);
        await context.SaveChangesAsync();
        var resultDto = mapper.Map<GetCollegesDto>(college);
        return Result<GetCollegesDto>.Success(resultDto);
    }

    public async Task<Result> DeleteCollegeAsync(int id)
    {
        var college = await context.Colleges.FindAsync(id);
        if (college == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"College with id {id} is not found"));
        }
        context.Colleges.Remove(college);
        await context.SaveChangesAsync();
        return Result.Success();
    }
}
