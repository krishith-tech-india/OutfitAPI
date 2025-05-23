﻿using Core;
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
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusService _orderStatusService;
        public OrderStatusController(IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ApiResponse> GetAllOrderStatus()
        {
            return new ApiResponse(HttpStatusCode.OK, await _orderStatusService.GetAllOrderStatusAsync());
        }

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ApiResponse> GetOrderStatusByID(int Id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _orderStatusService.GetOrderStatusByIDAsync(Id));
        }

        [Authorize]
        [HttpPost]
        public async Task<ApiResponse> AddOrderStatus([FromBody] OrderStatusDto orderStatusDto)
        {
            await _orderStatusService.InsertOrderStatusAsync(orderStatusDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<ApiResponse> DeleteOrderStatus(int Id)
        {
            await _orderStatusService.DeleteOrderStatusAsync(Id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("{Id}")]
        public async Task<ApiResponse> UpdateOrderStatus(int Id,[FromBody] OrderStatusDto orderStatusDto)
        {
            await _orderStatusService.UpdateOrderStatusAsync(Id, orderStatusDto);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
