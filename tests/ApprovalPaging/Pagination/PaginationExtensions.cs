using System;

namespace ApprovalPaging.Pagination;

public static class PaginationExtensions
{
    public static IPaginated<T> ToPaginated<T>(
        this IEnumerable<T> source,
        int pageSize,
        IComparer<T> comparer = null)
    {
        return new SortedListPaginator<T>(source, pageSize, comparer);
    }
}
