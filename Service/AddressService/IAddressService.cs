using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IAddressService
{
    Task<AddressDto> GetAddressByIdAsync(int id);
    Task<List<AddressDto>> GetAddressByUserIdAsync(int UserId, AddressFilterDto addressFilterDto);
    Task AddAddressAsync(AddressDto addressDto);
}
