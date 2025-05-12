using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using Dto;
using Microsoft.EntityFrameworkCore;

namespace Repo;

public class RoleRepo : BaseRepo<Role>, IRoleRepo
{
    private readonly IUserContext _userContext;

    public RoleRepo(OutfitDBContext context,IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }
    public async Task<List<Role>> GetAllRolesAsync(PaginationDto paginationDto)
    {
        return await Select(x => !x.IsDeleted).OrderBy(x => x.Id).Skip((paginationDto.PageNo - 1) * paginationDto.PageSize).Take(paginationDto.PageSize).ToListAsync();
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        var role = await GetByIdAsync(id);
        if (role == null || role.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound,string.Format(Constants.NotExistExceptionMessage,"Role" , "Id" , id));
        return role;
    }

    public async Task<bool> CheckIsRoleExistByNameAsync(string name)
    {
        return await AnyAsync(x => x.RoleName.ToLower().Equals(name.ToLower()) && !x.IsDeleted);
    }

    public async Task InsertRoleAsync(Role role)
    {
        if (string.IsNullOrWhiteSpace(role.RoleName))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest,string.Format(Constants.FieldrequiredExceptionMessage,"Role","Name"));
        if (await CheckIsRoleExistByNameAsync(role.RoleName))
            throw new ApiException(System.Net.HttpStatusCode.Conflict,string.Format(Constants.AleadyExistExceptionMessage, "Role", "Name", role.RoleName));
        role.AddedOn = DateTime.Now;
        role.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(role);
        await SaveChangesAsync();
    }

    public async Task<bool> CheckIsRoleIdExistAsync(int id)
    {
        return await AnyAsync(x => x.Id.Equals(id) && !x.IsDeleted);
    }

    public async Task UpdateRoleAsync(Role role)
    {
        if (string.IsNullOrWhiteSpace(role.RoleName))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Role", "Name"));
        role.LastUpdatedOn = DateTime.Now;
        role.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(role);
        await SaveChangesAsync();
    }


}
