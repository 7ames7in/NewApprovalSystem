using System;
using System.Collections;
using System.Collections.Generic;

namespace BuildingBlocks.Core.Pagination;

 public interface IPage<out T> : IEnumerable<T>
{
    int Ordinal { get; }
    int Count { get; }
    int PageSize { get; }
}