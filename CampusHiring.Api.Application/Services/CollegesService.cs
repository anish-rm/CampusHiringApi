using AutoMapper;
using AutoMapper.QueryableExtensions;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.College;
using CampusHiring.Api.Application.DTOs.Student;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Results;
using CampusHiring.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CampusHiring.Api.Application.Services;

public class CollegesService(CampusHiringDbContext context, IMapper mapper, IMemoryCache cache) : ICollegesService
{

    private const string CollegeListCacheName = "colleges_list_";
    private const string CollegeCacheName = "colleges_";
    public async Task<Result<IEnumerable<GetCollegesDto>>> GetCollegesAsync()
    {

        var cachekey = CollegeListCacheName;

        if(!cache.TryGetValue(cachekey, out IEnumerable<GetCollegesDto>? colleges))
        {
            colleges = await context.Colleges
               .AsNoTracking()
               .ProjectTo<GetCollegesDto>(mapper.ConfigurationProvider)
               .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            cache.Set(cachekey, colleges, cacheEntryOptions);
        }
        colleges ??= [];

        return Result<IEnumerable<GetCollegesDto>>.Success(colleges);
    }

    public async Task<Result<GetCollegesDto?>> GetCollegeAsync(int id)
    {

        var cacheKey = $"{CollegeCacheName}{id}";
        if(!cache.TryGetValue(cacheKey, out GetCollegesDto? college))
        {
            college = await context.Colleges
                .Where(c => c.Id == id)
                .ProjectTo<GetCollegesDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (college == null)
            {
                return Result<GetCollegesDto?>.NotFound(new Error(ErrorCodes.NotFound, $"College with id {id} is not found"));
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            cache.Set(cacheKey, college, cacheEntryOptions);
        }

      
        return Result<GetCollegesDto?>.Success(college);
    }

    public async Task<Result<IEnumerable<GetStudentDto>>> GetCollegeStudentsAsync(int collegeId)
    {
        var students = await context.Students
                        .Where(s => s.CollegeId == collegeId)
                        .ProjectTo<GetStudentDto>(mapper.ConfigurationProvider)
                        .ToListAsync();
        if(students.Count == 0)
        {
            var college = await context.Colleges.FindAsync(collegeId);
            if(college == null)
            {
                return Result<IEnumerable<GetStudentDto>>.NotFound(new Error(ErrorCodes.NotFound, $"College with id {collegeId} is not found"));
            }
        }
        return Result<IEnumerable<GetStudentDto>>.Success(students);
    }

    private void InvalidateCollegeCache(int id)
    {
        cache.Remove($"{CollegeCacheName}{id}");
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
        InvalidateCollegeCache(id);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<GetCollegesDto>> CreateCollegeAsync(CreateCollegeDto collegeDto)
    {
        var college = mapper.Map<College>(collegeDto);
        context.Colleges.Add(college);
        await context.SaveChangesAsync();
        cache.Remove(CollegeListCacheName);
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
        InvalidateCollegeCache(id);
        await context.SaveChangesAsync();
        return Result.Success();
    }
}
