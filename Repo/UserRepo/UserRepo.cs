using Core;
using Data.Contexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Repo;

public class UserRepo : BaseRepo<User>, IUserRepo
{
    public UserRepo(OutfitDBContext context) : base(context)
    {
    }

    public async Task<List<User>> GetAllUserAsync()
    {
        return await Select(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user == null || user.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"User id {id} not exist");
        return user;
    }

    public async Task<bool> CheckUserEmailExistOrNotAsync(string email)
    {
        return await AnyAsync(x => !x.IsDeleted && x.Email.Equals(email));
    }

    public async Task<bool> CheckUserPhoneNoExistOrNotAsync(string phoneNo)
    {
        return await AnyAsync(x => !x.IsDeleted && x.PhNo == phoneNo);
    }

    public async Task InsertUserAsync(User user)
    {
        await CheckDataValidOrnotAsync(user);
        user.AddedOn = DateTime.Now;
        //User.AddedBy = 0;
        await InsertAsync(user);
        await SaveChangesAsync();
    }


    public async Task UpdateUserAsync(User user)
    {
        await CheckDataValidOrnotAsync(user);
        user.LastUpdatedOn = DateTime.Now;
        //user.LastUpdatedBy = 0;
        Update(user);
        await SaveChangesAsync();
    }

    public async Task<bool> CheckUserExistUnderRoleIdAsync(int id)
    {
        return await AnyAsync(x => x.RoleId.Equals(id) && !x.IsDeleted);
    }

    //8.
    public async Task<User?> GetUserByEmailOrPhone(string emailOrPhone, string password)
    {
        return await GetQueyable().FirstOrDefaultAsync(x => x.Email.Equals(emailOrPhone) || x.PhNo == emailOrPhone);
    }

    public async Task<bool> CheckIsUserIdExistAsync(int userid)
    {
        return await AnyAsync(x => x.Id.Equals(userid) && !x.IsDeleted);
    }
    private async Task CheckDataValidOrnotAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"User name is required");
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"User Email is required");
        if (string.IsNullOrWhiteSpace(user.PhNo))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"User phone no is required");
        if (await ExistingUserPhonenoAndEmailUniqueOrNotAsync(user.PhNo, user.Email, user.Id))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"User email or phone no already registered");
    }
    private async Task<bool> ExistingUserPhonenoAndEmailUniqueOrNotAsync(string phoneNo, string email, int currentUserId)
    {
        return await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(currentUserId) && (x.Email.Equals(email) || x.PhNo == phoneNo));
    }
}
