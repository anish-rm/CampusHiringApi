using AutoMapper;
using AutoMapper.QueryableExtensions;
using CampusHiring.Api.Application.Contracts;
using CampusHiring.Api.Application.DTOs.Company;
using CampusHiring.Api.Application.DTOs.Interview;
using CampusHiring.Api.Common.Constants;
using CampusHiring.Api.Common.Results;
using CampusHiring.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CampusHiring.Api.Application.Services;

public class CompaniesService(CampusHiringDbContext dbContext, IMapper mapper) : ICompaniesService
{
    public async Task<Result<IEnumerable<GetCompanyDto>>> GetCompaniesAsync()
    {
        var companies = await dbContext.Companies
            .AsNoTracking()
            .ProjectTo<GetCompanyDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<IEnumerable<GetCompanyDto>>.Success(companies);
    }

    public async Task<Result<GetCompanyDto>> GetCompanyAsync(int id)
    {
        var company = await dbContext.Companies
            .Where(c => c.Id == id)
            .AsNoTracking()
            .ProjectTo<GetCompanyDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        if (company == null)
        {
            return Result<GetCompanyDto>.NotFound(new Error(ErrorCodes.NotFound, $"Company with id {id} not found"));
        }
        return Result<GetCompanyDto>.Success(company);
    }

    public async Task<Result> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto)
    {
        var company = await dbContext.Companies.FindAsync(id);
        if (company == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Company with id {id} not found"));
        }
        var isCompanyWithSameName = await dbContext.Companies.AnyAsync(c => c.Id != company.Id && c.Name == updateCompanyDto.Name);
        if (isCompanyWithSameName)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Company with name {updateCompanyDto.Name} already exists"));
        }
        var isCompanyWithSameEmail = await dbContext.Companies.AnyAsync(c => c.Id != company.Id && c.Email == updateCompanyDto.Email);
        if (isCompanyWithSameEmail)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Company with email {updateCompanyDto.Email} already exists"));
        }
        mapper.Map(updateCompanyDto, company);
        company.UpdatedAt = DateTime.Now;
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<GetCompanyDto>> CreateCompanyAsync(CreateCompanyDto companyDto)
    {
        var isCompanyWithSameName = await dbContext.Companies.AnyAsync(c => c.Name == companyDto.Name);
        if (isCompanyWithSameName)
        {
            return Result<GetCompanyDto>.NotFound(new Error(ErrorCodes.NotFound, $"Company with name {companyDto.Name} already exists"));
        }
        var isCompanyWithSameEmail = await dbContext.Companies.AnyAsync(c => c.Email == companyDto.Email);
        if (isCompanyWithSameEmail)
        {
            return Result<GetCompanyDto>.NotFound(new Error(ErrorCodes.NotFound, $"Company with email {companyDto.Email} already exists"));
        }
        var company = mapper.Map<Company>(companyDto);
        dbContext.Companies.Add(company);
        await dbContext.SaveChangesAsync();
        var resultDto = mapper.Map<GetCompanyDto>(company);
        return Result<GetCompanyDto>.Success(resultDto);
    }

    public async Task<Result> DeleteCompanyAsync(int id)
    {
        var company = await dbContext.Companies.FindAsync(id);
        if (company == null)
        {
            return Result.NotFound(new Error(ErrorCodes.NotFound, $"Company with id {id} not found"));
        }
        dbContext.Companies.Remove(company);
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<IEnumerable<GetInterviewerDto>>> GetInterviewersAsync(int companyId)
    {
        var interviewers = await dbContext.Interviewers
            .Where(i => i.CompanyId == companyId)
            .AsNoTracking()
            .ProjectTo<GetInterviewerDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<IEnumerable<GetInterviewerDto>>.Success(interviewers);
    }
}
