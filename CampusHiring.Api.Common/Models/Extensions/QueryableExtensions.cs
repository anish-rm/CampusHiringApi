using CampusHiring.Api.Common.Models.Paging;
using Microsoft.EntityFrameworkCore;

namespace CampusHiring.Api.Common.Models.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, PaginationParameter paginationParameter)
    {
        var totalCount = await source.CountAsync();

        var items = await source
            .Skip((paginationParameter.PageNumber - 1) * paginationParameter.PageSize)
            .Take(paginationParameter.PageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)paginationParameter.PageSize);

        var metadata = new PaginationMetadata
        {
            CurrentPage = paginationParameter.PageNumber,
            TotalCount = totalCount,
            PageSize = paginationParameter.PageSize,
            TotalPages = totalPages,
            HasNext = paginationParameter.PageNumber < totalPages,
            HasPrevious = paginationParameter.PageNumber > 1
        };

        return new PagedResult<T>
        {
            Data = items,
            Metadata = metadata
        };
    }
}
