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

    public async Task AddRoleAsync(RoleDto roleDto)
    {
        var roleEntity = _roleMapper.GetEntity(roleDto);
        roleEntity.AddedOn = DateTime.Now;
        roleEntity.LastUpdatedOn = DateTime.Now;

        var isExist = await _roleRepo.Any(x => x.RoleName.ToLower().Equals(roleDto.Name.ToLower()));
        if (isExist)
            throw new ApiException(System.Net.HttpStatusCode.Conflict, $"Role with name {roleDto.Name} aleady exist");

        await _roleRepo.InsertAsync(roleEntity);
        await _roleRepo.SaveChangesAsync();
    }

    public async Task<RoleDto> GetRoleByIdAsync(int id)
    {
        if (id <= 0)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"Invalid role id");
        var role = await _roleRepo.GetByIdAsync(id);
        if (role == null)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Role id {id} not exist");

        return _roleMapper.GetRoleDto(role);
    }

    public async Task<List<RoleDto>> GetRolesAsync()
    {
        try
        {
            var roles = await _roleRepo.Select(x => !x.IsDeleted.HasValue || !x.IsDeleted.Value).ToListAsync();
            return roles.Select(x => _roleMapper.GetRoleDto(x)).ToList();
        }
        catch(Exception ex)
        {
            throw;
        }
        
    }

    public async Task DeleteRoleAsync(int id)
    {
        if (id <= 0)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"Invalid role Id");
        var role = await _roleRepo.GetByIdAsync(id);
        if (role == null)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Role id {id} not exist");
        _roleRepo.Delete(role);
        await _roleRepo.SaveChangesAsync();
    }
}

//Controller => Service => repository