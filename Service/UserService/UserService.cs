using Core;
using Core.AppSettingConfigs;
using Core.Authentication;
using Data.Models;
using Dto;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repo;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
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

    public async Task<List<UserDto>> GetUsersAsync(UserFilterDto userFilterDto)
    {
        var userQueryable = _userRepo.GetQueyable();
        var roleQueyable = _roleRepo.GetQueyable();

        IQueryable<UserDto> UserQuery = userQueryable
            .Join(
                roleQueyable,
                user => user.RoleId,
                role => role.Id,
                (user, role) => new
                {
                    user.Id,
                    user.Name,
                    RoleId = role.Id,
                    user.Email,
                    user.PhNo,
                    role.RoleName,
                    RoleDeleted = role.IsDeleted,
                    user.IsDeleted
                }
            )
            .Where(x => !x.IsDeleted && !x.RoleDeleted)
            .Select(x => new UserDto()
             {
                 Id = x.Id,
                 Name = x.Name,
                 RoleId = x.RoleId,
                 Email = x.Email,
                 PhNo = x.PhNo,
                 RoleName = x.RoleName,
             });


        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(userFilterDto.GenericTextFilter))
            UserQuery = UserQuery.Where(x =>
                        x.Name.ToLower().Contains(userFilterDto.GenericTextFilter.ToLower()) ||
                        x.RoleName.ToLower().Contains(userFilterDto.GenericTextFilter.ToLower()) ||
                        x.Email.ToLower().Contains(userFilterDto.GenericTextFilter.ToLower()) ||
                        x.PhNo.ToLower().Contains(userFilterDto.GenericTextFilter.ToLower())
                    );

        //FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(userFilterDto.RoleNameFilterText))
            UserQuery = UserQuery.Where(x => x.RoleName.ToLower().Contains(userFilterDto.RoleNameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(userFilterDto.EmailFilterText))
            UserQuery = UserQuery.Where(x => x.Email.ToLower().Contains(userFilterDto.EmailFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(userFilterDto.PhNoFilterText))
            UserQuery = UserQuery.Where(x => x.PhNo.ToLower().Contains(userFilterDto.PhNoFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(userFilterDto.NameFilterText))
            UserQuery = UserQuery.Where(x => x.Name.ToLower().Contains(userFilterDto.NameFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(userFilterDto.OrderByField) && userFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.Name);
        else if (!string.IsNullOrWhiteSpace(userFilterDto.OrderByField) && userFilterDto.OrderByField.ToLower().Equals(Constants.OrderByRoleNameValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.RoleName);
        else if (!string.IsNullOrWhiteSpace(userFilterDto.OrderByField) && userFilterDto.OrderByField.ToLower().Equals(Constants.OrderByEmailValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.Email);
        else if (!string.IsNullOrWhiteSpace(userFilterDto.OrderByField) && userFilterDto.OrderByField.ToLower().Equals(Constants.OrderByPhoneNoValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.PhNo);
        else
            UserQuery = UserQuery.OrderBy(x => x.Id);

        //Pagination
        if (userFilterDto.IsPagination)
            UserQuery = UserQuery.Skip((userFilterDto.PageNo - 1) * userFilterDto.PageSize).Take(userFilterDto.PageSize);

        return await UserQuery.ToListAsync();

    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var userQuery = _userRepo.GetQueyable();
        var roleQuery = _roleRepo.GetQueyable();

        var user = await userQuery.Join(
                roleQuery,
                user => user.RoleId,
                role => role.Id,
                (user, role) => new
                {
                    user.Id,
                    user.Name,
                    RoleId = role.Id,
                    user.Email,
                    user.PhNo,
                    role.RoleName,
                    RoleDeleted = role.IsDeleted,
                    user.IsDeleted
                }
            )
            .Where(x => x.Id == id && !x.IsDeleted && !x.RoleDeleted)
            .Select(x => new UserDto()
            {
                Id = x.Id,
                Name = x.Name,
                RoleId = x.RoleId,
                Email = x.Email,
                PhNo = x.PhNo,
                RoleName = x.RoleName,
            }).FirstOrDefaultAsync();

        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "User", "Id", id));

        return user;
    }
    public async Task<List<UserDto>> GetUserByRoleIdAsync(int roleId,UserFilterDto userFilterDto)
    {
        if (!await _roleRepo.IsRoleIdExistAsync(roleId))
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Role", "Id", roleId));

        var userQueryable = _userRepo.GetQueyable();
        var roleQueyable = _roleRepo.GetQueyable();

        IQueryable<UserDto> UserQuery = userQueryable
            .Join(
                roleQueyable,
                user => user.RoleId,
                role => role.Id,
                (user, role) => new
                {
                    user.Id,
                    user.Name,
                    RoleId = role.Id,
                    user.Email,
                    user.PhNo,
                    role.RoleName,
                    RoleDeleted = role.IsDeleted,
                    user.IsDeleted
                }
            )
            .Where(x => x.RoleId == roleId && !x.IsDeleted && !x.RoleDeleted)
            .Select(x => new UserDto()
            {
                Id = x.Id,
                Name = x.Name,
                RoleId = x.RoleId,
                Email = x.Email,
                PhNo = x.PhNo,
                RoleName = x.RoleName,
            });


        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(userFilterDto.GenericTextFilter))
            UserQuery = UserQuery.Where(x =>
                        x.Name.ToLower().Contains(userFilterDto.GenericTextFilter.ToLower()) ||
                        x.RoleName.ToLower().Contains(userFilterDto.GenericTextFilter.ToLower()) ||
                        x.Email.ToLower().Contains(userFilterDto.GenericTextFilter.ToLower()) ||
                        x.PhNo.ToLower().Contains(userFilterDto.GenericTextFilter.ToLower())
                    );

        //FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(userFilterDto.RoleNameFilterText))
            UserQuery = UserQuery.Where(x => x.RoleName.ToLower().Contains(userFilterDto.RoleNameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(userFilterDto.EmailFilterText))
            UserQuery = UserQuery.Where(x => x.Email.ToLower().Contains(userFilterDto.EmailFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(userFilterDto.PhNoFilterText))
            UserQuery = UserQuery.Where(x => x.PhNo.ToLower().Contains(userFilterDto.PhNoFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(userFilterDto.NameFilterText))
            UserQuery = UserQuery.Where(x => x.Name.ToLower().Contains(userFilterDto.NameFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(userFilterDto.OrderByField) && userFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.Name);
        else if (!string.IsNullOrWhiteSpace(userFilterDto.OrderByField) && userFilterDto.OrderByField.ToLower().Equals(Constants.OrderByRoleNameValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.RoleName);
        else if (!string.IsNullOrWhiteSpace(userFilterDto.OrderByField) && userFilterDto.OrderByField.ToLower().Equals(Constants.OrderByEmailValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.Email);
        else if (!string.IsNullOrWhiteSpace(userFilterDto.OrderByField) && userFilterDto.OrderByField.ToLower().Equals(Constants.OrderByPhoneNoValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.PhNo);
        else
            UserQuery = UserQuery.OrderBy(x => x.Id);

        //Pagination
        if (userFilterDto.IsPagination)
            UserQuery = UserQuery.Skip((userFilterDto.PageNo - 1) * userFilterDto.PageSize).Take(userFilterDto.PageSize);

        return await UserQuery.ToListAsync();
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
}
