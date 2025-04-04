using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.Specification
{
    public class ProductWithBrandAndTypeSpecification : BaseSepcificatin<Product>
    {
        
        
        //GetAllWithSpecAsync
        public ProductWithBrandAndTypeSpecification(ProductSpecParams Params) :base
            (   P =>
                  (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
                  &&
                  (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
                    && 
                  (!Params.TypeId.HasValue  || P.ProductTypeId  == Params.TypeId)
            )
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
            if(!string.IsNullOrEmpty(Params.sort))
            {
                switch (Params.sort)
                {
                    case "priceAsc":
                        OrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        OrderByDescending(p => p.Price);
                        break;
                   
                    default:
                        OrderBy(p => p.Name);
                        break;
                }
            }
            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);

        }
        //GetbyIdWithSpecAsync
        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }

    }
}
