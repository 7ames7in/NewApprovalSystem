using System;
using System.Collections;
using System.Collections.Generic;

namespace BuildingBlocks.Core.Pagination;

public class ProjectingPage<T> : IPage<T>
{
    private readonly IReadOnlyList<T> _data;
    private readonly int _offset;
    private readonly int _count;

    public ProjectingPage(IReadOnlyList<T> data, int ordinal, int pageSize)
    {
        Ordinal = ordinal;
        PageSize = pageSize;
        _offset = (ordinal - 1) * pageSize;
        _count = Math.Max(0, Math.Min(pageSize, data.Count - _offset));
        _data = data;
    }

    public int Ordinal { get; }
    public int Count => _count;
    public int PageSize { get; }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
            yield return _data[_offset + i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
