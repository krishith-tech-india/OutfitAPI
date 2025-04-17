using Core;
using Dto;
using Mapper;
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

    public async Task AddRole(RoleDto roleDto)
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
}

//Controller => Service => repository