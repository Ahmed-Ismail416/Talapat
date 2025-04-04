using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatCore.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }


        //foreign key
        
        public int ProductTypeId { get; set; }
        public int ProductBrandId { get; set; }

        //navigational property
        public ProductType ProductType { get; set; }
        public ProductBrand ProductBrand { get; set; }

        
    }
}
