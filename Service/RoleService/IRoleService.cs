using System;
using Dto;

namespace Service;

public interface IRoleService
{
    Task AddRole(RoleDto roleDto);
}