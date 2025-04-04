using TalabatCore.Entities;

namespace Talapat.DTOs
{
    public class ProductToReturnDto : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        //foreign key
        public int ProductTypeId { get; set; }
        public int ProductBrandId { get; set; }
        //navigational property
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
    }
}
