using CampusHiring.Api.Application.DTOs.Assessment;
using CampusHiring.Api.Common.Results;

namespace CampusHiring.Api.Application.Contracts
{
    public interface IAssessmentsService
    {
        Task<Result<IEnumerable<GetAssessmentsDto>>> GetAssessmentsAsync();
        Task<bool> AssessmentExists(int id);
        Task<Result<GetAssessmentDto?>> GetAssessmentAsync(int id);
        Task<Result> UpdateAssessmentAsync(int id, UpdateAssessmentDto updateAssessmentDto);
        Task<Result<GetAssessmentDto>> CreateAssessmentAsync(CreateAssessmentDto createAssessmentDto);
        Task<Result> DeleteAssessmentAsync(int id);
    }
}