using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Backend.Core.Helpers
{

    public class PaginationQuery
    {
        const int maxPageSize = 20;

        private int _pageSize = 10; 


        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public PaginationQuery()
        {

        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
    
}
