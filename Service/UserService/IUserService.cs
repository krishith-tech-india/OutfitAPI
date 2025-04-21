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
    Task<List<UserDto>> GetUsersAsync();
    Task<UserDto> GetUserByIdAsync(int id);
    Task AddUserAsync(UserDto userDto);
    Task DeleteUserAsync(int id);
    Task UpadateUserAsync(int id, UserDto userDto);
}
