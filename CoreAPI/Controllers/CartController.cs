using Core;
using Dto;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("GetCarts/")]
        public async Task<ApiResponse> GetCarts([FromBody] CartFilterDto cartFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _cartService.GetCartsAsync(cartFilterDto));
        }

        [HttpPost("InsertCart/")]
        public async Task<ApiResponse> InsertCart([FromBody]CartDto cartDto)
        {
            await _cartService.InsertCartAsync(cartDto);
            return new ApiResponse(HttpStatusCode.OK);
        } 

        [HttpPut("UpdateCart/{id}")]
        public async Task<ApiResponse> UpdateCart(int id ,[FromBody] CartDto cartDto)
        {
            await _cartService.UpdateCartAsync(id, cartDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [HttpDelete("DeleteCart/{id}")]
        public async Task<ApiResponse> DeleteCart(int id)
        {
            await _cartService.DeleteCartAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
