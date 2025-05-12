using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IAddressRepo : IBaseRepo<Address>
{
    Task<List<Address>> GetAddressByUserIdAsync(int UserId,PaginationDto paginationDto);
    Task<Address> GetAddressByIdAsync(int id);
    Task InsertUserAsync(Address address);
}
