using System.Net;
using System.Threading.Tasks;
using Core;
using Dto;
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

        [HttpGet]
        public ApiResponse GetRole()
        {
            return new ApiResponse(HttpStatusCode.OK, _roleService.GetRolesAsync());
        }

        [HttpGet("{id}")]
        public ApiResponse GetRoles(int id)
        {
            return new ApiResponse(HttpStatusCode.OK, _roleService.GetRoleByIdAsync(id));
        }

        [HttpPost]
        public async Task<ApiResponse> AddRole(RoleDto roleDto)
        {
            await _roleService.AddRoleAsync(roleDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteRole(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return new ApiResponse(HttpStatusCode.OK);
        } 
    }
}
