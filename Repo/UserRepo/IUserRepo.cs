
using Data.Models;

namespace Repo;

public interface IUserRepo : IBaseRepo<User>
{
    Task<List<User>> GetAllUserAsync();
    Task<User> GetUserByIdAsync(int id);
    Task InsertUserAsync(User user);
    Task    UpdateUserAsync(User user);
    Task<bool> CheckUserExistUnderRoleIdAsync(int id);
    Task CheckDataValidOrnotAsync(User user);
    Task<bool> ExistingUserPhonenoAndEmailUniqueOrNotAsync(string phoneNo,string email ,int currentUserId);
    //7.
    Task<User?> GetUserByEmailOrPhone(string emailOrPhone, string password);
    Task<bool> CheckIsUserIdExistAsync(int userid);
}
