using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions;

public sealed record PageRequest(
    int Page = 1 ,
    int PageSize = 20 ,
    Sort? Sort = null)
{
    public long Skip => ((long) Page - 1) * PageSize;

    public PageRequest Normalize(int maxPageSize = 200)
    {
        var page = Page < 1 ? 1 : Page;
        var size = PageSize < 1 ? 1 : PageSize;
        if ( size > maxPageSize ) size = maxPageSize;

        return this with { Page = page , PageSize = size };
    }
}

public sealed record Sort(string Field , SortDirection Direction = SortDirection.Asc);

public enum SortDirection
{
    Asc = 0,
    Desc = 1
}
