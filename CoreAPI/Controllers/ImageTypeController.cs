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
        [HttpGet]
        public async Task<ApiResponse> GetAllImageType()
        {
            return new ApiResponse(HttpStatusCode.OK, await _imageTypeService.GetImageTypeAsync());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ApiResponse> GetImageTypeById(int id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _imageTypeService.GetImageTypeByIdAsync(id));
        }

        [Authorize]
        [HttpGet("/ImageTypeExistByName/{Name}")]
        public async Task<ApiResponse> IsImageTypeExistByName(string Name)
        {
            return new ApiResponse(HttpStatusCode.OK, await _imageTypeService.IsImageTypeExistByNameAsync(Name));
        }

        [Authorize]
        [HttpPost]
        public async Task<ApiResponse> AddImageType([FromBody] ImageTypeDto imageTypeDto)
        {
            await _imageTypeService.AddImageType(imageTypeDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteImageType(int id)
        {
            await _imageTypeService.DeleteImageTypeAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ApiResponse> UpdateImageType(int id, [FromBody] ImageTypeDto imageTypeDto)
        {
            await _imageTypeService.UpadateImageTypeAsync(id, imageTypeDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

    }
}
