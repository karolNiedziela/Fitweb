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
        public int PageNumber { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalItems { get; private set; }

        public bool HasPrevious => PageNumber > 1;

        public bool HasNext => PageNumber < TotalPages;

        public List<T> Items { get; set; } = new List<T>();

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalItems = count;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items.AddRange(items);
        }

        public async static Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();

            // Skip records based on current page number multiplied by page size and take number of records
            // based on page size
            // Example: currentPage = 2, pageSize = 10, so skip 10 elements and take next 10 elements
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
