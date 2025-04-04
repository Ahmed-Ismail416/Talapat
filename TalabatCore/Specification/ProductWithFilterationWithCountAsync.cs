using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.Specification
{
    public class ProductWithFilterationWithCountAsync: BaseSepcificatin<Product>
    {
        public ProductWithFilterationWithCountAsync(ProductSpecParams Params) : base
            (P =>
                (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
                &&
                (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
                &&
                (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            )
        {
           
            
        }
    }
   
}
