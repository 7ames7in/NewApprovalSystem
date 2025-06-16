using System;

namespace ApprovalPaging.Pagination;

 public interface IPage<out T> : IEnumerable<T>
{
    int Ordinal { get; }
    int Count { get; }
    int PageSize { get; }
}