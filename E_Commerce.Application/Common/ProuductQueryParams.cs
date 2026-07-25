using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class ProuductQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; } = default!;
        public string? Search { get; set; } = default!;
        public ProudectSortOptions? Sort { get; set; } = default!;
        public int PageIndex { get; set; } = 1;
        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;
        private int PageSize = DefaultPageSize;
        public int pageSize
        {
            get => PageSize;
            set => PageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? DefaultPageSize : value);
        }
    }
}
