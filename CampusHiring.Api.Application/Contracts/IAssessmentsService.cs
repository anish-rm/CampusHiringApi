using CampusHiring.Api.Application.DTOs.Assessment;
using CampusHiring.Api.Common.Enums;
using CampusHiring.Api.Common.Models.Filtering;
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
        Task<Result<GetAssessmentTypeDto>> CreateAssessmentTypeAsync(CreateAssessmentTypeDto assessmentTypeDto);
        Task<Result> DeleteAssessmentTypeAsync(int id);
        Task<Result> UpdateAssessmentTypeAsync(int id, UpdateAssessmentTypeDto assessmentTypeDto);
        Task<Result<GetAssessmentTypeDto>> GetAssessmentTypeAsync(int id);
        Task<Result<IEnumerable<GetAssessmentTypeDto>>> GetAssessmentTypesAsync();
        Task<Result> AssignAssessments(int collegeId, AssignAssessmentFilterParameter filter);
        Task<Result<IEnumerable<GetAssessmentDto>>> GetCollegeAssessmentsAsync(int collegeId, AssessmentFilterParameter filter);
        Task<List<string>> GetAssessmentClearedStudentIds(List<string> studentsIds, int previousRound, int companyId);
        Task<int> GetLastAssessmentRoundAsync(List<string> studentIds, int companyId);
    }
}