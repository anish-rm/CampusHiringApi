using CampusHiring.Api.Application.DTOs.Company;
using CampusHiring.Api.Application.DTOs.Interview;
using CampusHiring.Api.Common.Results;

namespace CampusHiring.Api.Application.Contracts
{
    public interface ICompaniesService
    {
        Task<Result<GetCompanyDto>> CreateCompanyAsync(CreateCompanyDto companyDto);
        Task<Result> DeleteCompanyAsync(int id);
        Task<Result<IEnumerable<GetCompanyDto>>> GetCompaniesAsync();
        Task<Result<GetCompanyDto>> GetCompanyAsync(int id);
        Task<Result<IEnumerable<GetInterviewerDto>>> GetInterviewersAsync(int companyId);
        Task<Result> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto);
    }
}