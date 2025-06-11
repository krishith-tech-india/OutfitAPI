using Core;
using Data.Models;
using Dto;
using Dto.Common;
using Dto.OrderStatus;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using Repo;
using System.Data;
using System.Linq.Expressions;

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

    public async Task<PaginatedList<RoleDto>> GetRolesAsync(RoleFilterDto roleFilterDto)
    {
        // Create paginated User List
        var paginatedRoleList = new PaginatedList<RoleDto>();

        // Create Predicates
        var roleFilterPredicate = PradicateBuilder.True<Role>();
        var userFilterPredicate = PradicateBuilder.True<User>();

        //Apply role is Deleted filter
        roleFilterPredicate = roleFilterPredicate.And(x => !x.IsDeleted);

        //Get address filters
        roleFilterPredicate = ApplyRoleFilters(roleFilterPredicate, roleFilterDto);

        //Apply filters
        var roleQueriable = _roleRepo.GetQueyable().Where(roleFilterPredicate);
        var userQueriable = _userRepo.GetQueyable().Where(userFilterPredicate);

        //join
        IQueryable<RoleDto> roleQuery = roleQueriable
            .GroupJoin(
                userQueriable,
                role => role.Id,
                user => user.RoleId,
                (role, userList) => new RoleDto()
                {
                    Id = role.Id,
                    Name = role.RoleName,
                    Description = role.RoleDesc,
                    UserCount = userList.Where(x => !x.IsDeleted).Count(),
                }
            );

        //ApplyGenericFilter
        roleQuery = ApplyGenericFilters(roleQuery, roleFilterDto);

        //OrderBy
        roleQuery = ApplyOrderByFilter(roleQuery, roleFilterDto);

        //FatchTotalCount
        paginatedRoleList.Count = await roleQuery.CountAsync();

        //Pagination
        roleQuery = ApplyPaginationFilter(roleQuery, roleFilterDto);

        //FatchItems
        paginatedRoleList.Items = await roleQuery.ToListAsync();

        return paginatedRoleList;
    }

    public async Task<RoleDto> GetRoleByIdAsync(int id)
    {
        var userQueriable = _userRepo.GetQueyable();
        var roleQueriable = _roleRepo.GetQueyable().Where(x => x.Id == id && !x.IsDeleted);

        var roleQuery = await roleQueriable
            .GroupJoin(
                userQueriable,
                role => role.Id,
                user => user.RoleId,
                (role, userList) => new RoleDto()
                {
                    Id = role.Id,
                    Name = role.RoleName,
                    Description = role.RoleDesc,
                    UserCount = userList.Where(x => !x.IsDeleted).Count(),
                }
            ).FirstOrDefaultAsync();

        if (roleQuery == null)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Role", "Id", id));

        return roleQuery;
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

    private IQueryable<RoleDto> ApplyGenericFilters(IQueryable<RoleDto> roleQuery, RoleFilterDto roleFilterDto)
    {
        //Generic filters
        if (!string.IsNullOrWhiteSpace(roleFilterDto.GenericTextFilter))
        {
            var genericFilterPredicate = PradicateBuilder.False<RoleDto>();
            var filterText = roleFilterDto.GenericTextFilter.Trim();
            genericFilterPredicate = genericFilterPredicate
                                    .Or(x => EF.Functions.ILike(x.Name, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Description, $"%{filterText}%"));

            //Apply generic filters
            return roleQuery.Where(genericFilterPredicate);
        }

        return roleQuery;
    }

    private Expression<Func<Role, bool>> ApplyRoleFilters(Expression<Func<Role, bool>> roleFilterPredicate, RoleFilterDto roleFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(roleFilterDto.NameFilterText))
            roleFilterPredicate = roleFilterPredicate.And(x => EF.Functions.ILike(x.RoleName, $"%{roleFilterDto.NameFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(roleFilterDto.DescriptionFilterText))
            roleFilterPredicate = roleFilterPredicate.And(x => EF.Functions.ILike(x.RoleDesc, $"%{roleFilterDto.DescriptionFilterText.Trim()}%"));
        return roleFilterPredicate;
    }

    private IQueryable<RoleDto> ApplyOrderByFilter(IQueryable<RoleDto> roleQuery, RoleFilterDto roleFilterDto)
    {
        var orderByMappings = new Dictionary<string, Expression<Func<RoleDto, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.OrderByNameValue, x => x.Name ?? "" },
            { Constants.OrderByDescriptionValue, x => x.Description ?? "" }
        };

        if (!orderByMappings.TryGetValue(roleFilterDto.OrderByField ?? "Id", out var orderByExpression))
        {
            orderByExpression = x => x.Id;
        }

        roleQuery = roleFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Desc)
            ? roleQuery.OrderByDescending(orderByExpression)
            : roleQuery.OrderBy(orderByExpression);

        return roleQuery;
    }

    private IQueryable<RoleDto> ApplyPaginationFilter(IQueryable<RoleDto> roleQuery, RoleFilterDto roleFilterDto)
    {
        if (roleFilterDto.IsPagination)
            roleQuery = roleQuery.Skip((roleFilterDto.PageNo - 1) * roleFilterDto.PageSize).Take(roleFilterDto.PageSize);

        return roleQuery;
    }
}