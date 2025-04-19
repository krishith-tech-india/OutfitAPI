using Core;
using Data.Models;
using Dto;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Repo;

namespace Service;

public class RoleService : IRoleService
{
    private readonly IRoleRepo _roleRepo;
    private readonly IRoleMapper _roleMapper;

    public RoleService(
        IRoleRepo roleRepo,
        IRoleMapper roleMapper
    )
    {
        _roleRepo = roleRepo;
        _roleMapper = roleMapper;
    }
    public async Task<List<RoleDto>> GetRolesAsync()
    {
            var roles = await _roleRepo.GetAllRolesAsync();
            return roles.Select(x => _roleMapper.GetRoleDto(x)).ToList();
    }
    public async Task<RoleDto> GetRoleByIdAsync(int id)
    {
        return _roleMapper.GetRoleDto(await _roleRepo.GetRoleByIdAsync(id));
    }
    public async Task AddRoleAsync(RoleDto roleDto)
    {
        var roleEntity = _roleMapper.GetEntity(roleDto);
        await _roleRepo.InsertRoleAsync(roleEntity);
    }
    public async Task DeleteRoleAsync(int id)
    {
        var role = await _roleRepo.GetRoleByIdAsync(id);
        role.IsDeleted = true;
        await _roleRepo.UpdateRoleAsync(role);
    }
    public async Task UpadateRoleAsync(int id, RoleDto roleDto)
    {
        var role = await _roleRepo.GetRoleByIdAsync(id);
        role.RoleName = roleDto.Name;
        role.RoleDesc = roleDto.Description;
        await _roleRepo.UpdateRoleAsync(role);
    }

}

//Controller => Service => repository