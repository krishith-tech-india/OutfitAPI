using Core;
using Data.Models;
using Dto;
using Dto.OrderStatus;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using Repo;
using System.Data;

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

    public async Task<List<RoleDto>> GetRolesAsync(RoleFilterDto roleFilterDto)
    {
        var userQueriable = _userRepo.GetQueyable();
        var roleQueriable = _roleRepo.GetQueyable();

        IQueryable<RoleDto> RoleQuery = roleQueriable
            .GroupJoin(
                userQueriable,
                role => role.Id,
                user => user.RoleId,
                (role, userList) => new
                {
                    role.Id,
                    role.RoleName,
                    role.RoleDesc,
                    role.IsDeleted,
                    UserCount = userList.Where(x => !x.IsDeleted).Count()
                }
            ).Where(x => !x.IsDeleted)
            .Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.RoleName,
                Description = x.RoleDesc,
                UserCount = x.UserCount
            });

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(roleFilterDto.GenericTextFilter))
            RoleQuery = RoleQuery.Where(x =>
                        x.Name.ToLower().Contains(roleFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(roleFilterDto.GenericTextFilter.ToLower()))
                    );

        //FieldTextFilterQuery
        if(!string.IsNullOrWhiteSpace(roleFilterDto.NameFilterText))
            RoleQuery = RoleQuery.Where(x => x.Name.ToLower().Contains(roleFilterDto.NameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(roleFilterDto.DescriptionFilterText))
            RoleQuery = RoleQuery.Where(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(roleFilterDto.DescriptionFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(roleFilterDto.OrderByField) && roleFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            RoleQuery = RoleQuery.OrderBy(x => x.Name);
        else if(!string.IsNullOrWhiteSpace(roleFilterDto.OrderByField) && roleFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            RoleQuery = RoleQuery.OrderBy(x => x.Description);
        else
            RoleQuery = RoleQuery.OrderBy(x => x.Id);

        //Pagination
        if (roleFilterDto.IsPagination)
            RoleQuery = RoleQuery.Skip((roleFilterDto.PageNo - 1) * roleFilterDto.PageSize).Take(roleFilterDto.PageSize);

        return await RoleQuery.ToListAsync();
    }

    public async Task<RoleDto> GetRoleByIdAsync(int id)
    {
        var userQueriable = _userRepo.GetQueyable();
        var roleQueriable = _roleRepo.GetQueyable();

        var RoleQuery = await roleQueriable
            .GroupJoin(
                userQueriable,
                role => role.Id,
                user => user.RoleId,
                (role, userList) => new
                {
                    role.Id,
                    role.RoleName,
                    role.RoleDesc,
                    role.IsDeleted,
                    UserCount = userList.Where(x => !x.IsDeleted).Count()
                }
            ).Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.RoleName,
                Description = x.RoleDesc,
                UserCount = x.UserCount
            }).FirstOrDefaultAsync();

        if (RoleQuery == null)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Role", "Id", id));

        return RoleQuery;
    }

    public async Task InsertRoleAsync(RoleDto roleDto)
    {
        var roleEntity = _roleMapper.GetEntity(roleDto);
        await _roleRepo.InsertRoleAsync(roleEntity);
    }

    public async Task UpadateRoleAsync(int id, RoleDto roleDto)
    {
        var role = await _roleRepo.GetRoleByIdAsync(id);
        role.RoleName = roleDto.Name;
        role.RoleDesc = roleDto.Description;
        await _roleRepo.UpdateRoleAsync(role);
    }

    public async Task DeleteRoleAsync(int id)
    {
        if (await _userRepo.IsUserExistUnderRoleIdAsync(id))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest,string.Format(Constants.DependentFindExceptionMessage, "Role Id"));
        var role = await _roleRepo.GetRoleByIdAsync(id);
        role.IsDeleted = true;
        await _roleRepo.UpdateRoleAsync(role);
    }

    public async Task<bool> IsRoleExistByNameAsync(string Name)
    {
        return await _roleRepo.IsRoleExistByNameAsync(Name);
    }
}