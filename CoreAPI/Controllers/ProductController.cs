using Core;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpPost("GetProducts/")]
        public async Task<ApiResponse> GetProducts([FromBody] ProductFilterDto productFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK,await _productService.GetProductsAsync(productFilterDto));
        }

        [Authorize]
        [HttpGet("GetProductByID/{Id}")]
        public async Task<ApiResponse> GetProductByID(int Id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _productService.GetProductByIDAsync(Id));
        }

        [Authorize]
        [HttpPost("GetProductByGroupId/{GroupId}")]
        public async Task<ApiResponse> GetProductByGroupId(int GroupId, [FromBody] ProductFilterDto productFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _productService.GetProductByGroupIdAsync(GroupId, productFilterDto));
        }

        [Authorize]
        [HttpPost("InsertProduct/")]
        public async Task<ApiResponse> InsertProduct([FromBody] ProductDto productDto)
        {
            await _productService.InsertProductAsync(productDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("UpdateProduct/{Id}")]
        public async Task<ApiResponse> UpdateProduct(int Id,[FromBody] ProductDto productDto)
        {
            await _productService.UpdateProductAsync(Id, productDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("DeleteProduct/{Id}")]
        public async Task<ApiResponse> DeleteProduct(int Id)
        {
            await _productService.DeleteProductAsync(Id);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
