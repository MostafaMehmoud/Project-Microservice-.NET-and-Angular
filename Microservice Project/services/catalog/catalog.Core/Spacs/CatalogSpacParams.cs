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
        public int pageIndex { get; set; } = 1;
        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? search { get; set; }
        public string? sort { get; set; }
        public string? brandId { get; set; }
        public string? typeId { get; set; }
    }
}
