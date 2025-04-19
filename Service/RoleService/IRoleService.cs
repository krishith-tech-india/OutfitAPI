using System;
using Core;
using Dto;

namespace Service;

public interface IRoleService
{
    Task AddRoleAsync(RoleDto roleDto);
    Task<List<RoleDto>> GetRolesAsync();
    Task DeleteRoleAsync(int id);
    Task<RoleDto> GetRoleByIdAsync(int id);
    Task UpadateRoleAsync(int id, RoleDto roleDto);
}