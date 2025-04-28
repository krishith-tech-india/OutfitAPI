using Core;
using Dto;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        public async Task<ApiResponse> GetaAllUser()
        {
            return new ApiResponse(HttpStatusCode.OK, await _userService.GetUsersAsync());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ApiResponse> GetUserById(int id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _userService.GetUserByIdAsync(id));
        }

        [HttpPost]
        public async Task<ApiResponse> AddUser([FromBody] UserDto userDto)
        {
            return new ApiResponse(HttpStatusCode.OK,await _userService.AddUserAsync(userDto));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ApiResponse> UpadateUser(int id, UserDto userDto)
        {
            await _userService.UpadateUserAsync(id, userDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [HttpPost("authenticate")]
        public async Task<ApiResponse> LoginUser([FromBody] AuthenticationDto authenticationDto)
        {
            // 4.
            string Token = await _userService.AuthenticateUserAndGetToken(authenticationDto);
            return new ApiResponse(HttpStatusCode.OK,Token);

        }
    }
}
