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
    public class ProductGroupController : ControllerBase
    {
        private readonly IProductGroupService _productGroupService;
        public ProductGroupController(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        [Authorize]
        [HttpPost("GetProductGroups/")]
        public async Task<ApiResponse> GetProductGroups([FromBody] ProductGroupFilterDto productGroupFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK,await _productGroupService.GetProductGroupsAsync(productGroupFilterDto));
        }

        [Authorize]
        [HttpGet("GetProductGroupByID/{Id}")]
        public async Task<ApiResponse> GetProductGroupByID(int Id)
        {
            return new ApiResponse(HttpStatusCode.OK,await _productGroupService.GetProductGroupByIDAsync(Id));
        }

        [Authorize]
        [HttpPost("GetProductGroupByCategoryId/{CategoryId}")]
        public async Task<ApiResponse> GetProductGroupByCategoryId(int CategoryId, [FromBody] ProductGroupFilterDto productGroupFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _productGroupService.GetProductGroupByCategoryIdAsync(CategoryId, productGroupFilterDto));
        }

        [Authorize]
        [HttpPost("InsertProductGroup/")]
        public async Task<ApiResponse> InsertProductGroup([FromBody]ProductGroupDto productGroupDto)
        {
            await _productGroupService.InsertProductGroupAsync(productGroupDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("UpdateProductGroup/{id}")]
        public async Task<ApiResponse> UpdateProductGroup(int id,[FromBody] ProductGroupDto productGroupDto)
        {
            await _productGroupService.UpdateProductGroupAsync(id, productGroupDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("DeleteProductGroup/{id}")]
        public async Task<ApiResponse> DeleteProductGroup(int id)
        {
            await _productGroupService.DeleteProductGroupAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpGet("IsProductGroupExistByName/{Name}")]
        public async Task<ApiResponse> IsProductGroupExistByName(string Name)
        {
            return new ApiResponse(HttpStatusCode.OK,await _productGroupService.IsProductGroupExistByName(Name));
        }
    }
}
