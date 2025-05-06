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

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _userRepo.GetAllUserAsync();
        return users.Select(x => _userMapper.GetUserDto(x)).ToList();
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        return _userMapper.GetUserDto(await _userRepo.GetUserByIdAsync(id));
    }
    public async Task<string> AddUserAsync(UserDto userDto)
    {
        if (!await _roleRepo.CheckIsRoleIdExistAsync(userDto.RoleId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Role Id {userDto.RoleId} is not exist");
        var userEntity = _userMapper.GetEntity(userDto);
        await _userRepo.InsertUserAsync(userEntity);
        return userEntity.GenerateTokenAsync(_jwtConfig);
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
            throw new ApiException(HttpStatusCode.NotFound, $"Role Id {userDto.RoleId} is not exist");
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
            throw new ApiException(HttpStatusCode.BadRequest, $"Email Or Phone is required");
        if(string.IsNullOrWhiteSpace(authDto.Password))
            throw new ApiException(HttpStatusCode.BadRequest, $"Password is required");
        var user = await _userRepo.GetUserByEmailOrPhone(authDto.EmailOrPhone, authDto.Password);
        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, "User Not found");
        return user.GenerateTokenAsync(_jwtConfig);
    }
}
