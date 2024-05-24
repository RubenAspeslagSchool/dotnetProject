using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Filters
{
    public class PaginationFilter
    {
        // TODO: read from config file
        const int _maxPageSize = 150;

        private int _pageSize = _maxPageSize;
        private int _pageNumber = 1;
       
        [JsonIgnore]
        public int MaxPageSize { get; set; } = _maxPageSize;

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

       
    }
}
