using Core;
using Dto;
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

        [HttpGet]
        public async Task<ApiResponse> GetAllOrderStatus()
        {
            return new ApiResponse(HttpStatusCode.OK,await _orderStatusService.GetAllOrderStatusAsync());
        }

        [HttpGet("{Id}")]
        public async Task<ApiResponse> GetOrderStatusByID(int Id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _orderStatusService.GetOrderStatusByIDAsync(Id));
        }

        [HttpPost]
        public async Task<ApiResponse> AddOrderStatus([FromBody]OrderStatusDto orderStatusDto)
        {
            await _orderStatusService.InsertOrderStatusAsync(orderStatusDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [HttpPut]
        public async Task<ApiResponse> UpdateOrderStatus([FromBody] OrderStatusDto orderStatusDto)
        {
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
