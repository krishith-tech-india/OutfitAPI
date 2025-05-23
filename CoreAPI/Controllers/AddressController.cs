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
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ApiResponse> GetAddressById(int id)
        {
            return new ApiResponse(HttpStatusCode.OK , await _addressService.GetAddressByIdAsync(id));
        }

        [Authorize]
        [HttpGet("UserId/{userId}")]
        public async Task<ApiResponse> GetAddressByUserId(int userId)
        {
            return new ApiResponse(HttpStatusCode.OK , await _addressService.GetAddressByUserIdAsync(userId));
        }

        [Authorize]
        [HttpPost]
        public async Task<ApiResponse> AddAddress([FromBody]AddressDto addressDto)
        {
            await _addressService.AddAddressAsync(addressDto);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
