using CampusHiring.Api.Common.Enums;

namespace CampusHiring.Api.Common.Models.Filtering;

public class AssignAssessmentFilterParameter
{
    public required int AssessmentTypeId {  get; set; }
    public required int Batch {  get; set; }
    public required IList<Department> Departments { get; set; }
    public int Round { get; set; } = 1;
}
