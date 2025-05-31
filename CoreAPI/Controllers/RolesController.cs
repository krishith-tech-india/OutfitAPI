using System.Net;
using System.Threading.Tasks;
using Core;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize]
        [HttpPost("GetRoles/")]
        public async Task<ApiResponse> GetRoles(RoleFilterDto roleFilterDto)
        {
            return new ApiResponse(HttpStatusCode.OK, await _roleService.GetRolesAsync(roleFilterDto));
        }

        [Authorize]
        [HttpGet("GetRoleById/{id}")]
        public async Task<ApiResponse> GetRoleById(int id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _roleService.GetRoleByIdAsync(id));
        }

        [Authorize]
        [HttpPost("InsertRole/")]
        public async Task<ApiResponse> InsertRole(RoleDto roleDto)
        {
            await _roleService.InsertRoleAsync(roleDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("UpadateRole/{id}")]
        public async Task<ApiResponse> UpadateRole(int id,RoleDto roleDto)
        {
            await _roleService.UpadateRoleAsync(id,roleDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("DeleteRole/{id}")]
        public async Task<ApiResponse> DeleteRole(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpGet("IsRoleExistByName/{name}")]
        public async Task<ApiResponse> IsRoleExistByName(string name)
        {
            return new ApiResponse(HttpStatusCode.OK, await _roleService.IsRoleExistByNameAsync(name));
        }
    }
}
