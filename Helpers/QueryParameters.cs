using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_api.Helpers
{
    public class QueryParameters
    {
        private int _maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? Search { get; set; } = null;
        public string? SortOrder { get; set; } = null;

        public QueryParameters Validate(){
            if(PageNumber < 1) PageNumber=1;
            if(PageSize < 1) PageSize=1;
            if(PageSize > _maxPageSize) PageSize=_maxPageSize;
            return this;
        }
    }
}