using System;
using Core;
using Dto;

namespace Service;

public interface IRoleService
{
    Task<List<RoleDto>> GetRolesAsync(RoleFilterDto roleFilterDto);
    Task<RoleDto> GetRoleByIdAsync(int id);
    Task AddRoleAsync(RoleDto roleDto);
    Task DeleteRoleAsync(int id);
    Task UpadateRoleAsync(int id, RoleDto roleDto);
}