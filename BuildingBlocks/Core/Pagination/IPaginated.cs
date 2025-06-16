using System.Collections.Generic;
using System.Collections;

namespace BuildingBlocks.Core.Pagination;

public interface IPaginated<out T> : IEnumerable<IPage<T>>
{
    int PagesCount { get; }
    IPage<T> this[int index] { get; }
}
