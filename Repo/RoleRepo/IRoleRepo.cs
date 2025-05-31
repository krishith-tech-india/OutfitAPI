
using Data.Models;
using Dto;

namespace Repo;

public interface IRoleRepo : IBaseRepo<Role>
{
    Task<Role> GetRoleByIdAsync(int id);
    Task<bool> IsRoleExistByNameAsync(string name);
    Task InsertRoleAsync(Role role);
    Task UpdateRoleAsync(Role role);
    Task<bool> IsRoleIdExistAsync(int id);
}
