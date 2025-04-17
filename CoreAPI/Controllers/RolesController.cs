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

        [HttpPost]
        public async Task<ApiResponse> AddRole(RoleDto roleDto)
        {
            await _roleService.AddRole(roleDto);
            return new ApiResponse(HttpStatusCode.OK);
        }

        //Get All
        //Get 
    }
}
