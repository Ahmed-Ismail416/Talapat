using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatCore.Specification
{
    public class ProductSpecParams
    {
        
        public string? sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

        private int pageSize = 5;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > 10 ? 10 : value;
        }

        public int PageIndex { get; set; } = 1;

        //SearchByName
        string? _search;
        public string? Search
        {
            get => _search;
            set => _search = value.ToLower();
        }

    }
}
