using System;
using System.Collections;
using System.Collections.Generic;

namespace BuildingBlocks.Core.Pagination;

public class SortedListPaginator<T> : IPaginated<T>
{
    private readonly Lazy<List<T>> _sortedData;
    private readonly int _pageSize;
    private readonly int _pagesCount;
    private readonly IComparer<T> _comparer;
    private int _currentPageNumber;

    public SortedListPaginator(IEnumerable<T> source, int pageSize, IComparer<T> comparer)
    {
        _pageSize = pageSize;
        _comparer = comparer ?? Comparer<T>.Default;

        var data = source.ToList();

        _sortedData = new Lazy<List<T>>(() =>
        {
            data.Sort(_comparer);
            return data;
        });

        _pagesCount = (data.Count + pageSize - 1) / pageSize;
        _currentPageNumber = 1; // Default to the first page
    }

    public int PagesCount => _pagesCount;

    public int PageNumber
    {
        get => _currentPageNumber;
        set
        {
            if (value < 1 || value > _pagesCount)
                throw new ArgumentOutOfRangeException(nameof(value), "Page number must be within valid range.");
            _currentPageNumber = value;
        }
    }

    public IPage<T> CurrentPage => this[_currentPageNumber - 1];

    public IPage<T> this[int index]
    {
        get
        {
            if (index < 0 || index >= _pagesCount)
                throw new ArgumentOutOfRangeException(nameof(index));

            return new ProjectingPage<T>(_sortedData.Value, index + 1, _pageSize);
        }
    }

    public IEnumerator<IPage<T>> GetEnumerator()
    {
        for (int i = 0; i < _pagesCount; i++)
            yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
