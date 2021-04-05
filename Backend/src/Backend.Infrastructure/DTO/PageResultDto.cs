using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.DTO
{
    public class PageResultDto<T>
    {
        public int TotalItems { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }

        public int TotalPages { get; set; }

        public List<T> Items { get; set; }
    }
}
