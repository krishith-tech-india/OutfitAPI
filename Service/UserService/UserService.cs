using Core;
using Dto;
using Mapper;
using Repo;


namespace Service;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IUserMapper _userMapper;
    private readonly IRoleRepo _roleRepo;

    public UserService(
        IUserRepo userRepo,
        IUserMapper userMapper,
        IRoleRepo roleRepo

    )
    {
        _userRepo = userRepo;
        _userMapper = userMapper;
        _roleRepo = roleRepo;
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
    public async Task AddUserAsync(UserDto userDto)
    {
        if (!await _roleRepo.CheckIsRoleIdExistAsync(userDto.RoleId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Role Id {userDto.RoleId} is not exist");
        var userEntity = _userMapper.GetEntity(userDto);
        await _userRepo.InsertUserAsync(userEntity);
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
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Role Id {userDto.RoleId} is not exist");
        var user = await _userRepo.GetUserByIdAsync(id);
        user.RoleId = userDto.RoleId;
        user.Email = userDto.Email;
        user.PhNo = userDto.PhNo;
        user.Name = userDto.Name;
        await _userRepo.UpdateUserAsync(user);
    }
}
