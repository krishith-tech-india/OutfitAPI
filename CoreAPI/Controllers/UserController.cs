using Core;
using Dto;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ApiResponse> GetaAllUser()
        {
            return new ApiResponse(HttpStatusCode.OK, await _userService.GetUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse> GetUserById(int id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _userService.GetUserByIdAsync(id));
        }

        [HttpPost]
        public async Task<ApiResponse> AddUser(UserDto userDto)
        {
            await _userService.AddUserAsync(userDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse> UpadateUser(int id, UserDto userDto)
        {
            await _userService.UpadateUserAsync(id, userDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

    }
}
