using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public class UserMapper : IUserMapper
{
    public User GetEntity(UserDto userDto)
    {
        return new User
        {
            RoleId = userDto.RoleId,
            Email = userDto.Email,
            PhNo = userDto.PhNo,
            Name = userDto.Name,
        };
    }

    public UserDto GetUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            RoleId = user.RoleId,
            Email = user.Email,
            PhNo = user.PhNo,
            Name = user.Name,
        };
    }
}
