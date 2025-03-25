using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }

        public PagedResponse(T data, int PageNumber, int PageSize)
        {
            this.PageNumber = PageNumber;
            this.PageSize = PageSize;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }

        public PagedResponse(T data, int PageNumber, int PageSize, int TotalPages, int TotalElements)
        {
            this.PageNumber = PageNumber;
            this.PageSize = PageSize;
            this.TotalPages = TotalPages;
            this.TotalElements = TotalElements;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
