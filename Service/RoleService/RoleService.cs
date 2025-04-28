using Core;
using Data.Models;
using Dto;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Repo;

namespace Service;

public class RoleService : IRoleService
{
    private readonly IRoleRepo _roleRepo;
    private readonly IRoleMapper _roleMapper;
    private readonly IUserRepo _userRepo;

    public RoleService(
        IRoleRepo roleRepo,
        IRoleMapper roleMapper,
        IUserRepo userRepo
    )
    {
        _roleRepo = roleRepo;
        _roleMapper = roleMapper;
        _userRepo = userRepo;
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
        if (await _userRepo.CheckUserExistUnderRoleIdAsync(id))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"User Id available In {id} Role Id.");
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