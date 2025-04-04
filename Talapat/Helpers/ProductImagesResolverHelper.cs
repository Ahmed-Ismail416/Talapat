using AutoMapper;
using TalabatCore.Entities;
using Talapat.DTOs;

namespace Talapat.Helpers
{
    public class ProductImagesResolverHelper : IValueResolver<Product, ProductToReturnDto, string>
    {
        public ProductImagesResolverHelper(IConfiguration config)
        {
            _Config = config;
        }

        public IConfiguration _Config { get; }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!String.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_Config["ApiBaseUrl"]}{source.PictureUrl}";
            }
            return String.Empty;
        }
    }
}
