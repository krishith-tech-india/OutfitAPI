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
        [HttpPost("GetUsers/")]
        public async Task<ApiResponse> GetUsers(UserFilterDto userFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _userService.GetUsersAsync(userFilterDto));
        }

        [Authorize]
        [HttpGet("GetUserById/{id}")]
        public async Task<ApiResponse> GetUserById(int id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _userService.GetUserByIdAsync(id));
        }

        [Authorize]
        [HttpPost("GetUserByRoleId/")]
        public async Task<ApiResponse> GetUserByRoleId(int roleId, [FromBody] UserFilterDto userFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _userService.GetUserByRoleIdAsync(roleId, userFilterDto));
        }

        [HttpPost("InsertUser/")]
        public async Task<ApiResponse> InsertUser([FromBody] UserDto userDto)
        {
            return new ApiResponse(HttpStatusCode.OK,await _userService.InsertUserAsync(userDto));
        }

        [Authorize]
        [HttpPut("UpadateUser/{id}")]
        public async Task<ApiResponse> UpadateUser(int id, UserDto userDto)
        {
            await _userService.UpadateUserAsync(id, userDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("DeleteUser/{id}")]
        public async Task<ApiResponse> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [HttpPost("AuthorizeUser/")]
        public async Task<ApiResponse> AuthorizeUser([FromBody] AuthenticationDto authenticationDto)
        {
            string Token = await _userService.AuthenticateUserAndGetToken(authenticationDto);
            return new ApiResponse(HttpStatusCode.OK,Token);

        }

        [Authorize]
        [HttpGet("IsUserPhoneNumberExistAsync/{phoneNumber}")]
        public async Task<ApiResponse> IsUserPhoneNumberExist(string phoneNumber)
        {
            return new ApiResponse(HttpStatusCode.OK, await _userService.IsUserPhoneNumberExistAsync(phoneNumber));
        }

        [Authorize]
        [HttpGet("IsUserEmailExistAsync/{email}")]
        public async Task<ApiResponse> IsUserEmailExist(string email)
        {
            return new ApiResponse(HttpStatusCode.OK, await _userService.IsUserEmailExistAsync(email));
        }
    }
}
