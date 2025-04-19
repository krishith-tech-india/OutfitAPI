using Core;
using Data.Contexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Repo;

public class RoleRepo : BaseRepo<Role>, IRoleRepo
{
    public RoleRepo(OutfitDBContext context) : base(context)
    {
        
    }
    public async Task<List<Role>> GetAllRolesAsync()
    {
        return await Select(x => x.IsDeleted == false).ToListAsync();
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        var role = await GetByIdAsync(id);
        if (role == null || role.IsDeleted == true)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Role id {id} not exist");
        return role;
    }

    public async Task<bool> CheckIsRoleExistByNameAsync(string name)
    {
        return await AnyAsync(x => x.RoleName.ToLower().Equals(name.ToLower()) && !x.IsDeleted.Value);
    }

    public async Task InsertRoleAsync(Role role)
    {
        if (role.RoleName == null)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"Role name is required");
        if (await CheckIsRoleExistByNameAsync(role.RoleName))
            throw new ApiException(System.Net.HttpStatusCode.Conflict, $"Role with name {role.RoleName} aleady exist");
        role.AddedOn = DateTime.Now;
        //role.AddedBy = 0
        await InsertAsync(role);
        await SaveChangesAsync();
    }

    // not complate
    public async Task UpdateRoleAsync(Role role)
    {
        if (role.RoleName == null)
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"Role name is required");
        role.LastUpdatedOn = DateTime.Now;
        //role.LastUpdatedBy = 0;
        Update(role);
        await SaveChangesAsync();
    }


}
