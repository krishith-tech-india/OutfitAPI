using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IUserService
{
    Task<bool> IsUserEmailExistAsync(string email);
    Task<bool> IsUserPhoneNumberExistAsync(string phoneNo);
    Task<List<UserDto>> GetUsersAsync(UserFilterDto userFilterDto);
    Task<UserDto> GetUserByIdAsync(int id);
    Task<string> InsertUserAsync(UserDto userDto);
    Task DeleteUserAsync(int id);
    Task UpadateUserAsync(int id, UserDto userDto);
    Task<string> AuthenticateUserAndGetToken(AuthenticationDto authDto);
    Task<List<UserDto>> GetUserByRoleIdAsync(int roleId, UserFilterDto userFilterDto);
}
