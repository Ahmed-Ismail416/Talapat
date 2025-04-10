using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatCore.Entities;
using TalabatCore.Repositories;
using TalabatCore.Specification;
using Talapat.DTOs;
using Talapat.Errors;
using Talapat.Helpers;

namespace Talapat.Controllers
{
    
    public class ProductsController : BaseAPIController
    {
        public ProductsController(IGenericRepository<Product> genericRepository, IMapper mapper,
            IGenericRepository<ProductBrand> GenericRepository,
            IGenericRepository<ProductType> genericRepositoryTypes)
        {
            _GenericRepository = genericRepository;
            _Mapper = mapper;
            _GenericRepositoryBrands = GenericRepository;
            _GenericRepositoryTypes = genericRepositoryTypes;
        }

        public IGenericRepository<Product> _GenericRepository { get; }
        public IMapper _Mapper { get; }
        public IGenericRepository<ProductBrand> _GenericRepositoryBrands { get; }
        public IGenericRepository<ProductType> _GenericRepositoryTypes { get; }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
           var spec = new ProductWithBrandAndTypeSpecification(Params);
           var products = await _GenericRepository.GetAllWithSpecAsync(spec);
            // products is null
            if (products is null)
                return NotFound(new ApiResponse(404));
            var countSpec = new ProductWithBrandAndTypeSpecification(Params);
            var count = await _GenericRepository.GetCountWithSpecAsync(countSpec);
            var GetMapper = _Mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var GetPagination = new Pagination<ProductToReturnDto>
            {
                Data = GetMapper,
                PageIndex = Params.PageIndex,
                PageSize = Params.PageSize,
                Count = count
            };
            return Ok(GetPagination);
        }

        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] // لا تفض 2 وا دنوف تون روريلاا نم نيتجاح ريغ شيلعحرتبم تنيوب دنلاا ناشع يد سوتيتسلااب لود تويبرتا 222 يكوا 
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await _GenericRepository.GetEntityWithSpecAsync(spec);
            if(product is null)
                return NotFound(new ApiResponse(404));
            var ProductDto = _Mapper.Map<Product,ProductToReturnDto>(product);
            return Ok(ProductDto);
        }


        //GetAllBrands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrand()
        {
            var Brands = await _GenericRepositoryBrands.GetAllAsync();
            return Ok(Brands);
        }

        //GetAllTypes
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            var Types = await _GenericRepositoryTypes.GetAllAsync();
            return Ok(Types);
        }
    }
}
