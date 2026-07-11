using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.ProductsDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #region Get

        // Get All Products
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery] ProuductQueryParams queryParams, CancellationToken ct)
        {
            var result = await _productService.GetAllProductsAsync(queryParams, ct);
             
            return ToActionResult(result);
        }
        // Get Product By Id
        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct( int id,CancellationToken ct)
        {
            var result = await _productService.GetProductByIdAsync(id ,ct);
            return ToActionResult(result);
        }
        // Get All Brands
        // GET: api/brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct)
        {
            var result = await _productService.GetAllBrandsAsync(ct);
            return ToActionResult(result);
        }
        // Get All Types
        // GET: api/Products/types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct)
        {
            var result = await _productService.GetAllTypesAsync(ct);
            return ToActionResult(result);
        }
        #endregion

    }
}
