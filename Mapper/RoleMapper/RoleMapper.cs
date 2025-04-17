using System;
using Data.Models;
using Dto;

namespace Mapper;

public class RoleMapper : IRoleMapper
{
    public Role GetEntity(RoleDto roleDto)
    {
        return new Role
        {
            RoleName = roleDto.Name,
            RoleDesc = roleDto.Description
        };
    }
}
