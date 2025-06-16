using System;

namespace ApprovalPaging.Pagination;

public interface IPaginated<out T> : IEnumerable<IPage<T>>
{
    int PagesCount { get; }
    IPage<T> this[int index] { get; }
}
