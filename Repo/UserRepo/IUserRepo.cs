
using Data.Models;
using Dto;

namespace Repo;

public interface IUserRepo : IBaseRepo<User>
{
    Task<bool> CheckUserEmailExistOrNotAsync(string email); 
    Task<bool> CheckUserPhoneNoExistOrNotAsync(string phoneNo);
    Task<User> GetUserByIdAsync(int id);
    Task InsertUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<bool> CheckUserExistUnderRoleIdAsync(int id);
    Task<User?> GetUserByEmailOrPhone(string emailOrPhone, string password);
    Task<bool> CheckIsUserIdExistAsync(int userid);
}
