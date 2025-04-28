using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IAddressRepo : IBaseRepo<Address>
{
    void CheckDataValidOrnot(Address address);
    Task<List<Address>> GetAddressByUserIdAsync(int UserId);
    Task<Address> GetAddressByIdAsync(int id);
    Task InsertUserAsync(Address address);
}
