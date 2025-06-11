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
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [Authorize]
        [HttpGet("GetAddressById/{id}")]
        public async Task<ApiResponse> GetAddressById(int id)
        {
            return new ApiResponse(HttpStatusCode.OK , await _addressService.GetAddressByIdAsync(id));
        }

        [Authorize]
        [HttpPost("GetAddressByUserId/")]
        public async Task<ApiResponse> GetAddressByUserId(int userId,[FromBody] AddressFilterDto addressFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK , await _addressService.GetAddressByUserIdAsync(userId,addressFilterDto));
        }

        [Authorize]
        [HttpPost("InsertAddress/")]
        public async Task<ApiResponse> InsertAddress([FromBody]AddressDto addressDto)
        {
            await _addressService.InsertAddressAsync(addressDto);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
