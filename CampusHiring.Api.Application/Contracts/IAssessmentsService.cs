using CampusHiring.Api.Application.DTOs;

namespace CampusHiring.Api.Application.Contracts
{
    public interface IAssessmentsService
    {
        Task<GetAssessmentDto> CreateAssessmentAsync(CreateAssessmentDto createAssessmentDto);
        Task DeleteAssessmentAsync(int id);
        Task<GetAssessmentDto?> GetAssessmentAsync(int id);
        Task<IEnumerable<GetAssessmentsDto>> GetAssessmentsAsync();
        Task UpdateAssessmentAsync(int id, UpdateAssessmentDto updateAssessmentDto);
        Task<bool> AssessmentExists(int id);
    }
}