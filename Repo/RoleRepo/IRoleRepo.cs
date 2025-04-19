
using Data.Models;

namespace Repo;

public interface IRoleRepo : IBaseRepo<Role>
{
    Task<bool> CheckIsRoleExistByNameAsync(string name);
    Task<List<Role>> GetAllRolesAsync();
    Task<Role> GetRoleByIdAsync(int id);
    Task InsertRoleAsync(Role role);
    Task UpdateRoleAsync(Role role);
}
