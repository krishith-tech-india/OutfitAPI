using Core;
using Data.Models;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [Authorize]
        [HttpPost("GetProductCategorys/")]
        public async Task<ApiResponse> GetProductCategorys([FromBody] ProductCategoryFilterDto productCategoryFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _productCategoryService.GetProductCategorysAsync(productCategoryFilterDto));
        }

        [Authorize]
        [HttpGet("GetProductCategoryByID/{id}")]
        public async Task<ApiResponse> GetProductCategoryByID(int id)
        {
            return new ApiResponse(HttpStatusCode.OK,await _productCategoryService.GetProductCategoryByIdAsync(id));
        }

        [Authorize]
        [HttpPost("InsertProductCategory/")]
        public async Task<ApiResponse> InsertProductCategory([FromBody]ProductCategoryDto productCategoryDto)
        {
            await _productCategoryService.InsertProductCategoryAsync(productCategoryDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("UpdateProductCategory/{id}")]
        public async Task<ApiResponse> UpdateProductCategory(int id,[FromBody] ProductCategoryDto productCategoryDto)
        {
            await _productCategoryService.UpdateProductCategoryAsync(id,productCategoryDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("DeleteProductCategory/{id}")]
        public async Task<ApiResponse> DeleteProductCategory(int id)
        {
            await _productCategoryService.DeleteProductCategoryAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpGet("ProductCategoryExistByName/{Name}")]
        public async Task<ApiResponse> IsProductCategoryExistByName(string Name)
        {
            return new ApiResponse(HttpStatusCode.OK,await _productCategoryService.IsProductCategoryExistByNameAsync(Name));
        }
    }
}
