using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Helpers
{
    // inspired by https://code-maze.com/paging-aspnet-core-webapi/

    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int currentPage, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public async static Task<PagedList<T>> ToPagedList(IQueryable<T> source, int currentPage, int pageSize)
        {
            var count = source.Count();

            // Skip records based on current page number multiplied by page size and take number of records
            // based on page size
            // Example: currentPage = 2, pageSize = 10, so skip 10 elements and take next 10 elements
            var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, currentPage, pageSize);
        }
    }
}
