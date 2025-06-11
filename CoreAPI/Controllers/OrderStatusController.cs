using Core;
using Dto;
using Dto.OrderStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusService _orderStatusService;
        public OrderStatusController(IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        [Authorize]
        [HttpPost("GetOrderStatus/")]
        public async Task<ApiResponse> GetOrderStatus([FromBody] OrderStatusFilterDto orderStatusFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _orderStatusService.GetOrderStatusAsync(orderStatusFilterDto));
        }

        [Authorize]
        [HttpGet("GetOrderStatusByID/{Id}")]
        public async Task<ApiResponse> GetOrderStatusByID(int Id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _orderStatusService.GetOrderStatusByIdAsync(Id));
        }

        [Authorize]
        [HttpPost("InsertOrderStatus/")]
        public async Task<ApiResponse> InsertOrderStatus([FromBody] OrderStatusDto orderStatusDto)
        {
            await _orderStatusService.InsertOrderStatusAsync(orderStatusDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("UpdateOrderStatus/{Id}")]
        public async Task<ApiResponse> UpdateOrderStatus(int Id,[FromBody] OrderStatusDto orderStatusDto)
        {
            await _orderStatusService.UpdateOrderStatusAsync(Id, orderStatusDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("DeleteOrderStatus/{Id}")]
        public async Task<ApiResponse> DeleteOrderStatus(int Id)
        {
            await _orderStatusService.DeleteOrderStatusAsync(Id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpGet("OrderStatusExistByName/{Name}")]
        public async Task<ApiResponse> IsOrderStatusExistByName(string Name)
        {
            return new ApiResponse(HttpStatusCode.OK, await _orderStatusService.IsOrderStatusExistByNameAsync(Name));
        }

    }
}
