namespace CampusHiring.Api.Common.Models.Filtering;

public class BaseFilterParameter
{
    public string? Search {  get; set; }
    public string? SortBy { get; set; }
    public bool SortDescendng { get; set; } = false;
}
