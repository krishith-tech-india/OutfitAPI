
using Data.Models;

namespace Repo;

public interface IRoleRepo : IBaseRepo<Role>
{
    Task<List<Role>> GetAllRolesAsync();
    Task<Role> GetRoleByIdAsync(int id);
    Task<bool> CheckIsRoleExistByNameAsync(string name);
    Task InsertRoleAsync(Role role);
    Task UpdateRoleAsync(Role role);
    Task<bool> CheckIsRoleIdExistAsync(int id);
}
