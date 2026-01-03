using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Core.Spacs
{
    public class CatalogSpacParams
    {
        private const int MaxPageSize = 80;
        private int _pageSize = 10;
        public int PageIndex { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? Search { get; set; }
        public string? Sort { get; set; }
        public string? BrandId { get; set; }
        public string? TypeId { get; set; }
    }
}
