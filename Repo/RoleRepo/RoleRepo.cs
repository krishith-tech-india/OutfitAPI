using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using Dto;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Xml.Linq;

namespace Repo;

public class RoleRepo : BaseRepo<Role>, IRoleRepo
{
    private readonly IUserContext _userContext;

    public RoleRepo(OutfitDBContext context,IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        var role = await GetByIdAsync(id);
        if (role == null || role.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound,string.Format(Constants.NotExistExceptionMessage,"Role" , "Id" , id));
        return role;
    }

    public async Task<bool> IsRoleExistByNameAsync(string name)
    {
        return await AnyAsync(x => !x.IsDeleted && x.RoleName.ToLower().Equals(name.ToLower()));
    }

    public async Task InsertRoleAsync(Role role)
    {
        await IsRoleDataValidAsync(role);
        role.AddedOn = DateTime.Now;
        role.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(role);
        await SaveChangesAsync();
    }

    public async Task UpdateRoleAsync(Role role)
    {
        await IsRoleDataValidAsync(role);
        role.LastUpdatedOn = DateTime.Now;
        role.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(role);
        await SaveChangesAsync();
    }

    public async Task<bool> IsRoleIdExistAsync(int id)
    {
        return await AnyAsync(x => x.Id.Equals(id) && !x.IsDeleted);
    }

    private async Task IsRoleDataValidAsync(Role role)
    {
        if (string.IsNullOrWhiteSpace(role.RoleName))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Role", "Name"));
        if (await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(role.Id) && x.RoleName.ToLower().Equals(role.RoleName.ToLower())))
            throw new ApiException(System.Net.HttpStatusCode.Conflict, string.Format(Constants.AleadyExistExceptionMessage, "Role", "Name", role.RoleName));
    }
}
