using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions;

public sealed class PagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }

    public required int Page { get; init; }
    public required int PageSize { get; init; }

    /// <summary>Total number of items across all pages.</summary>
    public required long TotalCount { get; init; }

    public long TotalPages =>
        PageSize <= 0 ? 0 : (long) Math.Ceiling((double) TotalCount / PageSize);

    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    public int? PreviousPage => HasPrevious ? Page - 1 : null;
    public int? NextPage => HasNext ? Page + 1 : null;

    public static PagedResult<T> Empty(int page , int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 1 : pageSize;

        return new()
        {
            Items = Array.Empty<T>() ,
            Page = page ,
            PageSize = pageSize ,
            TotalCount = 0
        };
    }
}
