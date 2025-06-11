using System;
using Core;
using Dto;
using Dto.Common;

namespace Service;

public interface IRoleService
{
    Task<PaginatedList<RoleDto>> GetRolesAsync(RoleFilterDto roleFilterDto);
    Task<RoleDto> GetRoleByIdAsync(int id);
    Task InsertRoleAsync(RoleDto roleDto);
    Task DeleteRoleAsync(int id);
    Task UpadateRoleAsync(int id, RoleDto roleDto);
    Task<bool> IsRoleExistByNameAsync(string Name);
}