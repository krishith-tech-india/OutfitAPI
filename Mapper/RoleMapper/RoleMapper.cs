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

    public RoleDto GetRoleDto(Role role)
    {
        return new RoleDto
        {

            Id = role.Id,
            Name = role.RoleName,
            Description = role.RoleDesc,
        };
    }
}
