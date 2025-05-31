using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using Dto;
using Microsoft.EntityFrameworkCore;

namespace Repo;

public class UserRepo : BaseRepo<User>, IUserRepo
{
    private readonly IUserContext _userContext;
    public UserRepo(OutfitDBContext context,IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user == null || user.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound,string.Format(Constants.NotExistExceptionMessage, "User", "Id", id));
        return user;
    }

    public async Task InsertUserAsync(User user)
    {
        await IsUserDataValidAsync(user);
       
            user.AddedOn = DateTime.Now;
            if (_userContext.loggedInUser.Id != 0)
                user.AddedBy = _userContext.loggedInUser.Id;
            await InsertAsync(user);
            await SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        await IsUserDataValidAsync(user);
        user.LastUpdatedOn = DateTime.Now;
        user.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(user);
        await SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmailOrPhone(string emailOrPhone, string password)
    {
        return await GetQueyable().FirstOrDefaultAsync(x => (x.Email.ToLower().Equals(emailOrPhone.ToLower()) || x.PhNo == emailOrPhone) && !x.IsDeleted);
    }

    public async Task<bool> IsUserEmailExistAsync(string email)
    {
        return await AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()));
    }

    public async Task<bool> IsUserPhoneNumberExistAsync(string phoneNo)
    {
        return await AnyAsync(x => x.PhNo == phoneNo);
    }

    public async Task<bool> IsUserExistUnderRoleIdAsync(int id)
    {
        return await AnyAsync(x => x.RoleId.Equals(id) && !x.IsDeleted);
    }

    public async Task<bool> IsUserIdExistAsync(int userid)
    {
        return await AnyAsync(x => x.Id.Equals(userid) && !x.IsDeleted);
    }

    private async Task IsUserDataValidAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest,string.Format(Constants.FieldrequiredExceptionMessage,"User" , "Name"));
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest,string.Format(Constants.FieldrequiredExceptionMessage, "User", "Email"));
        if (string.IsNullOrWhiteSpace(user.PhNo))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest,string.Format(Constants.FieldrequiredExceptionMessage, "User", "Phone No."));
        if (await ExistingUserPhonenoAndEmailUniqueOrNotAsync(user.PhNo, user.Email, user.Id))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest,string.Format(Constants.AleadyExistExceptionMessage,"User", "Email OR Phone No.", ""));
    }

    private async Task<bool> ExistingUserPhonenoAndEmailUniqueOrNotAsync(string phoneNo, string email, int currentUserId)
    {
        return await AnyAsync(x => !x.Id.Equals(currentUserId) && (x.Email.ToLower().Equals(email.ToLower()) || x.PhNo == phoneNo));
    }
}
