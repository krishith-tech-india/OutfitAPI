
using Data.Models;
using Dto;

namespace Repo;

public interface IUserRepo : IBaseRepo<User>
{
    Task<bool> IsUserEmailExistAsync(string email); 
    Task<bool> IsUserPhoneNumberExistAsync(string phoneNo);
    Task<User> GetUserByIdAsync(int id);
    Task InsertUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<bool> IsUserExistUnderRoleIdAsync(int id);
    Task<User?> GetUserByEmailOrPhone(string emailOrPhone, string password);
    Task<bool> IsUserIdExistAsync(int userid);
}
