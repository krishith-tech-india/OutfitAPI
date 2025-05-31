using System;
using Data.Models;
using Dto;

namespace Mapper;

public interface IRoleMapper
{
    Role GetEntity(RoleDto roleDto);
    RoleDto GetRoleDto(Role role);
}
