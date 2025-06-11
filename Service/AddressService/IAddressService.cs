using Dto;
using Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IAddressService
{
    Task<AddressDto> GetAddressByIdAsync(int id);
    Task<PaginatedList<AddressDto>> GetAddressByUserIdAsync(int UserId, AddressFilterDto addressFilterDto);
    Task InsertAddressAsync(AddressDto addressDto);
}
