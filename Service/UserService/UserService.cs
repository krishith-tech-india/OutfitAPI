using Core;
using Core.AppSettingConfigs;
using Core.Authentication;
using Data.Models;
using Dto;
using Dto.Common;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repo;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;


namespace Service;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IUserMapper _userMapper;
    private readonly IRoleRepo _roleRepo;
    private readonly JWTConfigrations _jwtConfig;
    public UserService(
        IUserRepo userRepo,
        IUserMapper userMapper,
        IRoleRepo roleRepo,
        IOptions<JWTConfigrations> jwtConfig

    )
    {
        _userRepo = userRepo;
        _userMapper = userMapper;
        _roleRepo = roleRepo;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<PaginatedList<UserDto>> GetUsersAsync(UserFilterDto userFilterDto)
    {
        //Create paginated User List
        var paginatedUserList = new PaginatedList<UserDto>();

        //create Predicates
        var userFilterPredicate = PradicateBuilder.True<User>();
        var roleFilterPredicate = PradicateBuilder.True<Role>();

        //Apply user id filter
        userFilterPredicate = userFilterPredicate.And(x => !x.IsDeleted);
        roleFilterPredicate = roleFilterPredicate.And(x => !x.IsDeleted);

        //Get address filters
        userFilterPredicate = ApplyUserFilters(userFilterPredicate, userFilterDto);

        //Get user filters
        roleFilterPredicate = ApplyRoleFilters(roleFilterPredicate, userFilterDto);

        //Apply filters
        var userQueriable = _userRepo.GetQueyable().Where(userFilterPredicate);
        var roleQueyable = _roleRepo.GetQueyable().Where(roleFilterPredicate);

        //join
        IQueryable<UserDto> userQuery = userQueriable.
            Join(roleQueyable,
                user => user.RoleId,
                role => role.Id,
                (user, role) => new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    RoleId = user.RoleId,
                    Email = user.Email,
                    PhNo = user.PhNo,
                    RoleName = role.RoleName,
                }
            );

        //ApplyGenericFilter
        userQuery = ApplyGenericFilters(userQuery, userFilterDto);

        //OrderBy
        userQuery = ApplyOrderByFilter(userQuery, userFilterDto);

        //FatchTotalCount
        paginatedUserList.Count = await userQuery.CountAsync();

        //Pagination
        userQuery = ApplyPaginationFilter(userQuery, userFilterDto);

        //FatchItems
        paginatedUserList.Items = await userQuery.ToListAsync();

        return paginatedUserList;
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {

        var userQuery = _userRepo.GetQueyable().Where(x => x.Id == id && !x.IsDeleted);
        var roleQuery = _roleRepo.GetQueyable().Where(x => !x.IsDeleted);

        var user = await userQuery.Join(
                roleQuery,
                user => user.RoleId,
                role => role.Id,
                (user, role) => new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    RoleId = user.RoleId,
                    Email = user.Email,
                    PhNo = user.PhNo,
                    RoleName = role.RoleName,
                }
            ).FirstOrDefaultAsync();

        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "User", "Id", id));

        return user;
    }
    public async Task<PaginatedList<UserDto>> GetUserByRoleIdAsync(int roleId,UserFilterDto userFilterDto)
    {
        if (!await _roleRepo.IsRoleIdExistAsync(roleId))
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Role", "Id", roleId));

        //Create paginated User List
        var paginatedUserList = new PaginatedList<UserDto>();

        //create Predicates
        var userFilterPredicate = PradicateBuilder.True<User>();
        var roleFilterPredicate = PradicateBuilder.True<Role>();

        //Apply user id filter
        userFilterPredicate = userFilterPredicate.And(x => x.RoleId.Equals(roleId));
        userFilterPredicate = userFilterPredicate.And(x => !x.IsDeleted);
        roleFilterPredicate = roleFilterPredicate.And(x => !x.IsDeleted);

        //Get address filters
        userFilterPredicate = ApplyUserFilters(userFilterPredicate, userFilterDto);

        //Get user filters
        roleFilterPredicate = ApplyRoleFilters(roleFilterPredicate, userFilterDto);

        //Apply filters
        var userQueriable = _userRepo.GetQueyable().Where(userFilterPredicate);
        var roleQueyable = _roleRepo.GetQueyable().Where(roleFilterPredicate);

        //join
        IQueryable<UserDto> userQuery = userQueriable.
            Join(roleQueyable,
                user => user.RoleId,
                role => role.Id,
                (user, role) => new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    RoleId = user.RoleId,
                    Email = user.Email,
                    PhNo = user.PhNo,
                    RoleName = role.RoleName,
                }
            );

        //ApplyGenericFilter
        userQuery = ApplyGenericFilters(userQuery, userFilterDto);

        //OrderBy
        userQuery = ApplyOrderByFilter(userQuery, userFilterDto);

        //FatchTotalCount
        paginatedUserList.Count = await userQuery.CountAsync();

        //Pagination
        userQuery = ApplyPaginationFilter(userQuery, userFilterDto);

        //FatchItems
        paginatedUserList.Items = await userQuery.ToListAsync();

        return paginatedUserList;
    }

    public async Task<string> InsertUserAsync(UserDto userDto)
    {
        if (!await _roleRepo.IsRoleIdExistAsync(userDto.RoleId))
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Role", "Id", userDto.RoleId));
        var userEntity = _userMapper.GetEntity(userDto);
        await _userRepo.InsertUserAsync(userEntity);
        return userEntity.GenerateTokenAsync(_jwtConfig);
    }
    public async Task UpadateUserAsync(int id, UserDto userDto)
    {
        if (!await _roleRepo.IsRoleIdExistAsync(userDto.RoleId))
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage,"Role", "Id" , userDto.RoleId));
        var user = await _userRepo.GetUserByIdAsync(id);
        user.RoleId = userDto.RoleId;
        user.Email = userDto.Email;
        user.PhNo = userDto.PhNo;
        user.Name = userDto.Name;
        await _userRepo.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepo.GetUserByIdAsync(id);
        user.IsDeleted = true;
        await _userRepo.UpdateUserAsync(user);
    }


    public async Task<string> AuthenticateUserAndGetToken(AuthenticationDto authDto)
    {
        if (string.IsNullOrWhiteSpace(authDto.EmailOrPhone))
            throw new ApiException(HttpStatusCode.BadRequest,string.Format(Constants.FieldrequiredExceptionMessage,"User", "Email Or Phone"));
        if(string.IsNullOrWhiteSpace(authDto.Password))
            throw new ApiException(HttpStatusCode.BadRequest,string.Format(Constants.FieldrequiredExceptionMessage,"User", "Passsword"));
        var user = await _userRepo.GetUserByEmailOrPhone(authDto.EmailOrPhone, authDto.Password);
        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound,string.Format(Constants.UnauthorizedExceptionMessage));
        return user.GenerateTokenAsync(_jwtConfig);
    }

    public async Task<bool> IsUserEmailExistAsync(string email)
    {
        return await _userRepo.IsUserEmailExistAsync(email);
    }

    public async Task<bool> IsUserPhoneNumberExistAsync(string phoneNo)
    {
        return await _userRepo.IsUserPhoneNumberExistAsync(phoneNo);
    }

    private IQueryable<UserDto> ApplyGenericFilters(IQueryable<UserDto> userQuery, UserFilterDto userFilterDto)
    {
        //Generic filters
        if (!string.IsNullOrWhiteSpace(userFilterDto.GenericTextFilter))
        {
            var genericFilterPredicate = PradicateBuilder.False<UserDto>();
            var filterText = userFilterDto.GenericTextFilter.Trim();
            genericFilterPredicate = genericFilterPredicate
                                    .Or(x => EF.Functions.ILike(x.Name, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.RoleName, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Email, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.PhNo, $"%{filterText}%"));

            //Apply generic filters
            return userQuery.Where(genericFilterPredicate);
        }

        return userQuery;
    }

    private Expression<Func<User, bool>> ApplyUserFilters(Expression<Func<User, bool>> userFilterPredicate, UserFilterDto userFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(userFilterDto.EmailFilterText))
            userFilterPredicate = userFilterPredicate.And(x => EF.Functions.ILike(x.Email, $"%{userFilterDto.EmailFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(userFilterDto.PhNoFilterText))
            userFilterPredicate = userFilterPredicate.And(x => EF.Functions.ILike(x.PhNo, $"%{userFilterDto.PhNoFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(userFilterDto.NameFilterText))
            userFilterPredicate = userFilterPredicate.And(x => EF.Functions.ILike(x.Name, $"%{userFilterDto.NameFilterText.Trim()}%"));

        return userFilterPredicate;
    }

    private Expression<Func<Role, bool>> ApplyRoleFilters(Expression<Func<Role, bool>> roleFilterPredicate, UserFilterDto userFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(userFilterDto.RoleNameFilterText))
            roleFilterPredicate = roleFilterPredicate.And(x => EF.Functions.ILike(x.RoleName, $"%{userFilterDto.RoleNameFilterText.Trim()}%"));

        return roleFilterPredicate;
    }

    private IQueryable<UserDto> ApplyOrderByFilter(IQueryable<UserDto> userQuery, UserFilterDto userFilterDto)
    {
        var orderByMappings = new Dictionary<string, Expression<Func<UserDto, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.OrderByNameValue, x => x.Name ?? "" },
            { Constants.OrderByRoleNameValue, x => x.RoleName ?? "" },
            { Constants.OrderByEmailValue, x => x.Email ?? "" },
            { Constants.OrderByPhoneNoValue, x => x.PhNo ?? "" },
        };

        if (!orderByMappings.TryGetValue(userFilterDto.OrderByField ?? "Id", out var orderByExpression))
        {
            orderByExpression = x => x.Id;
        }

        userQuery = userFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Desc)
            ? userQuery.OrderByDescending(orderByExpression)
            : userQuery.OrderBy(orderByExpression);

        return userQuery;
    }

    private IQueryable<UserDto> ApplyPaginationFilter(IQueryable<UserDto> userQuery, UserFilterDto userFilterDto)
    {
        if (userFilterDto.IsPagination)
            userQuery = userQuery.Skip((userFilterDto.PageNo - 1) * userFilterDto.PageSize).Take(userFilterDto.PageSize);

        return userQuery;
    }
}
