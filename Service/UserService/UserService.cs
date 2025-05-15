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

    public async Task<List<UserDto>> GetUsersAsync(GenericFilterDto genericFilterDto)
    {
        var userQuery = _userRepo.GetQueyable();
        var roleQuery = _roleRepo.GetQueyable();

        IQueryable<UserDto> UserQuery = userQuery
            .Join(
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


        //TextQuery
        if (!string.IsNullOrWhiteSpace(genericFilterDto.GenericTextFilter))
            UserQuery = UserQuery.Where(x =>
                        x.Name.ToLower().Contains(genericFilterDto.GenericTextFilter) ||
                        x.RoleName.ToLower().Contains(genericFilterDto.GenericTextFilter) ||
                        x.Email.ToLower().Contains(genericFilterDto.GenericTextFilter) ||
                        x.PhNo.ToLower().Contains(genericFilterDto.GenericTextFilter)
                    );

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.Name);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByRoleNameValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.RoleName);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByEmailValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.Email);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByPhoneNoValue, StringComparison.OrdinalIgnoreCase))
            UserQuery = UserQuery.OrderBy(x => x.PhNo);
        else
            UserQuery = UserQuery.OrderBy(x => x.Id);

        //Pagination
        if (genericFilterDto.IsPagination)
            UserQuery = UserQuery.Skip((genericFilterDto.PageNo - 1) * genericFilterDto.PageSize).Take(genericFilterDto.PageSize);

        var Query = UserQuery.ToQueryString();

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
    public async Task<string> AddUserAsync(UserDto userDto)
    {
        if (!await _roleRepo.CheckIsRoleIdExistAsync(userDto.RoleId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Role Id {userDto.RoleId} is not exist");
        var userEntity = _userMapper.GetEntity(userDto);
        await _userRepo.InsertUserAsync(userEntity);
        return userEntity.GenerateTokenAsync(_jwtConfig);
    }

    public async Task<bool> CheckUserEmailExistOrNotAsync(string email)
    {
        return await _userRepo.CheckUserEmailExistOrNotAsync(email);
    }

    public async Task<bool> CheckUserPhoneNoExistOrNotAsync(string phoneNo)
    {
        return await _userRepo.CheckUserPhoneNoExistOrNotAsync(phoneNo);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepo.GetUserByIdAsync(id);
        user.IsDeleted = true;
        await _userRepo.UpdateUserAsync(user);
    }
    public async Task UpadateUserAsync(int id, UserDto userDto)
    {
        if (!await _roleRepo.CheckIsRoleIdExistAsync(userDto.RoleId))
            throw new ApiException(HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage,"Role", "Id" , userDto.RoleId));
        var user = await _userRepo.GetUserByIdAsync(id);
        user.RoleId = userDto.RoleId;
        user.Email = userDto.Email;
        user.PhNo = userDto.PhNo;
        user.Name = userDto.Name;
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
}
