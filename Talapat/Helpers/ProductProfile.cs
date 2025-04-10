using AutoMapper;
using TalabatCore.Entities;
using TalabatCore.Entities.Identity;
using Talapat.DTOs;
using Talapat.DTOs.Role;
using TalabatCore.Entities.Order;
using Talapat.DTOs.Order;
using TalabatCore.Entities.Basket;
using Talapat.DTOs.Basket;

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
            CreateMap<TalabatCore.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, TalabatCore.Entities.Order.Address>();
            CreateMap<RoleDto, AppRole>().ReverseMap()
                .ForMember(r => r.Id, opt => opt.Ignore())
                .ForMember(r => r.NormalizedName, opt => opt.Ignore());

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(o => o.DeliveryMethod, opt => opt.MapFrom(o => o.DeliveryMethod.ShortName))
                .ForMember(o => o.DeliveryMethodCost, opt => opt.MapFrom(o => o.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(o => o.ProductId, opt => opt.MapFrom(o => o.Product.ProductId))
                .ForMember(o => o.ProductName, opt => opt.MapFrom(o => o.Product.ProductName))
                .ForMember(o => o.PictureUrl, opt => opt.MapFrom(o => o.Product.PictureUrl))
                .ForMember(o => o.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<CustomerBasket, CustomerBasketDto>();
        }
    }
}
