using CampusHiring.Api.Application.DTOs.Interview;
using CampusHiring.Api.Common.Results;
using Microsoft.AspNetCore.JsonPatch;

namespace CampusHiring.Api.Application.Contracts
{
    public interface IInterviewsService
    {
        Task<Result<GetInterviewerAvailabilityDto>> CreateInterviewerAvailabilityAsync(int companyId, CreateInterviewerAvailabilityDto interviewerAvailabilityDto);
        Task<Result<GetInterviewRoundDto>> CreateInterviewRoundAsync(int companyId, CreateInterviewRoundDto roundDto);
        Task<Result> DeleteInterviewRoundAsync(int id);
        Task<Result<IEnumerable<GetCandidateSelectionDto>>> GetCandidateSelectionsAsync();
        Task<Result<IEnumerable<GetCandidateSelectionDto>>> GetCollegeCandidateSelectionsAsync(int collegeId);
        Task<Result<IEnumerable<GetInterviewDto>>> GetCollegeInterviewsAsync(int collegeId);
        Task<Result<IEnumerable<GetInterviewRoundDto>>> GetCompanyInterviewRoundsAsync(int id);
        Task<Result<GetInterviewDto>> GetInterviewAsync(int id);
        Task<Result<IEnumerable<GetInterviewerAvailabilityDto>>> GetInterviewersAvailabilityAsync();
        Task<Result<GetInterviewRoundDto>> GetInterviewRoundAsync(int id);
        Task<Result<IEnumerable<GetInterviewRoundDto>>> GetInterviewRoundsAsync();
        Task<Result<IEnumerable<GetInterviewDto>>> GetInterviewsAsync();
        Task<Result> ScheduleInterviews(int companyId, int collegeId, int batch, DateTime interviewDate,  int duration = 60, int roundNumber = 1);
        Task<Result> SubmitFeedbackAsync(int id, int companyId, JsonPatchDocument<PatchInterviewDto> patchDoc);
        Task<Result> UpdateInterviewAsync(int id, UpdateInterviewDto interviewDto);
        Task<Result> UpdateInterviewRoundAsync(int id, int companyId, UpdateInterviewRoundDto roundDto);
    }
}