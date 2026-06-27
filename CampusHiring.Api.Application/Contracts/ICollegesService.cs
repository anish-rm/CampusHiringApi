using CampusHiring.Api.Application.DTOs.College;
using CampusHiring.Api.Common.Results;

namespace CampusHiring.Api.Application.Contracts
{
    public interface ICollegesService
    {
        Task<Result<GetCollegesDto>> CreateCollegeAsync(CreateCollegeDto collegeDto);
        Task<Result> DeleteCollegeAsync(int id);
        Task<Result<GetCollegesDto>> GetCollegeAsync(int id);
        Task<Result<IEnumerable<GetCollegesDto>>> GetCollegesAsync();
        Task<Result> UpdateCollegeAsync(int id, UpdateCollegeDto collegeDto);
    }
}