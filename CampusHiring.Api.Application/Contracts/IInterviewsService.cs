using CampusHiring.Api.Application.DTOs.Interview;
using CampusHiring.Api.Common.Results;

namespace CampusHiring.Api.Application.Contracts
{
    public interface IInterviewsService
    {
        Task<Result<GetInterviewerAvailabilityDto>> CreateInterviewerAvailabilityAsync(CreateInterviewerAvailabilityDto interviewerAvailabilityDto);
        Task<Result<GetInterviewRoundDto>> CreateInterviewRoundAsync(CreateInterviewRoundDto roundDto);
        Task<Result> DeleteInterviewRoundAsync(int id);
        Task<Result<IEnumerable<GetInterviewRoundDto>>> GetCompanyInterviewRoundsAsync(int id);
        Task<Result<GetInterviewDto>> GetInterviewAsync(int id);
        Task<Result<IEnumerable<GetInterviewerAvailabilityDto>>> GetInterviewersAvailabilityAsync();
        Task<Result<GetInterviewRoundDto>> GetInterviewRoundAsync(int id);
        Task<Result<IEnumerable<GetInterviewRoundDto>>> GetInterviewRoundsAsync();
        Task<Result<IEnumerable<GetInterviewDto>>> GetInterviewsAsync();
        Task<Result> ScheduleInterviews(int companyId, int collegeId, DateTime interviewDate, int duration = 60, int roundNumber = 1);
        Task<Result> UpdateInterviewRoundAsync(int id, UpdateInterviewRoundDto roundDto);
    }
}