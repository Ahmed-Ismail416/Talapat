using AutoMapper;
using TalabatCore.Entities;
using TalabatCore.Entities.Identity;
using Talapat.DTOs;

namespace Talapat.Helpers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product,ProductToReturnDto>()
                .ForMember(P => P.ProductBrand, O => O.MapFrom(P => P.ProductBrand.Name))
                .ForMember(P => P.ProductType, O => O.MapFrom(P => P.ProductType.Name))
                .ForMember(p => p.PictureUrl, O => O.MapFrom<ProductImagesResolverHelper>());
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
