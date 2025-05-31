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
    Task InsertAddressAsync(Address address);
}
