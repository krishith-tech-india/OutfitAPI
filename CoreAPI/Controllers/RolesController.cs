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
        [HttpGet]
        public async Task<ApiResponse> GetRole()
        {
            return new ApiResponse(HttpStatusCode.OK, await _roleService.GetRolesAsync());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ApiResponse> GetRoleById(int id)
        {
            return new ApiResponse(HttpStatusCode.OK, await _roleService.GetRoleByIdAsync(id));
        }

        [Authorize]
        [HttpPost]
        public async Task<ApiResponse> AddRole(RoleDto roleDto)
        {
            await _roleService.AddRoleAsync(roleDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteRole(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ApiResponse> UpadateRole(int id,RoleDto roleDto)
        {
            await _roleService.UpadateRoleAsync(id,roleDto);
            return new ApiResponse(HttpStatusCode.OK);
        }
    }
}
