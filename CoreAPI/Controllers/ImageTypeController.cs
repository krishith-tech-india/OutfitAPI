using Core;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Service;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageTypeController : ControllerBase
    {
        private readonly IImageTypeService _imageTypeService;

        public ImageTypeController(IImageTypeService imageTypeService)
        {
            _imageTypeService = imageTypeService;
        }

        [Authorize]
        [HttpPost("GetImageTypes/")]
        public async Task<ApiResponse> GetImageTypes(ImageTypeFilterDto imageTypeFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _imageTypeService.GetImageTypesAsync(imageTypeFilterDto));
        }

        [Authorize]
        [HttpGet("GetImageTypeById/{id}")]
        public async Task<ApiResponse> GetImageTypeById(int id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _imageTypeService.GetImageTypeByIdAsync(id));
        }

        [Authorize]
        [HttpPost("InsertImageType/")]
        public async Task<ApiResponse> InsertImageType([FromBody] ImageTypeDto imageTypeDto)
        {
            await _imageTypeService.InsertImageTypeAsync(imageTypeDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("UpdateImageType/{id}")]
        public async Task<ApiResponse> UpdateImageType(int id, [FromBody] ImageTypeDto imageTypeDto)
        {
            await _imageTypeService.UpadateImageTypeAsync(id, imageTypeDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("DeleteImageType/{id}")]
        public async Task<ApiResponse> DeleteImageType(int id)
        {
            await _imageTypeService.DeleteImageTypeAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpGet("IsImageTypeExistByName/{Name}")]
        public async Task<ApiResponse> IsImageTypeExistByName(string Name)
        {
            return new ApiResponse(HttpStatusCode.OK, await _imageTypeService.IsImageTypeExistByNameAsync(Name));
        }
    }
}
