using System.ComponentModel.DataAnnotations;

namespace CampusHiring.Api.Common.Models.Paging;

public class PaginationParameter
{
    private const int MaxPageSize = 50;

    private int _pageSize = 10;

    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
    public int PageNumber { get; set; }
    [Range(1, MaxPageSize, ErrorMessage = "Page number must be between 1 and 50")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value <= MaxPageSize ? value : MaxPageSize;
    }
}
