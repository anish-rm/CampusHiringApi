namespace CampusHiring.Api.Common.Models.Filtering;

public class AssessmentFilterParameter : BaseFilterParameter
{
    public int? Batch {  get; set; }
    public string? Result { get; set; }
    public int? MinScore { get; set; }
    public int? MaxScore { get; set; }
}
