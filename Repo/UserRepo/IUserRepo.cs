
using Data.Models;

namespace Repo;

public interface IUserRepo : IBaseRepo<User>
{
    Task<bool> CheckUserEmailExistOrNotAsync(string email); 
    Task<bool> CheckUserPhoneNoExistOrNotAsync(string phoneNo);
    Task<List<User>> GetAllUserAsync();
    Task<User> GetUserByIdAsync(int id);
    Task InsertUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<bool> CheckUserExistUnderRoleIdAsync(int id);
    Task<User?> GetUserByEmailOrPhone(string emailOrPhone, string password);
    Task<bool> CheckIsUserIdExistAsync(int userid);
}
