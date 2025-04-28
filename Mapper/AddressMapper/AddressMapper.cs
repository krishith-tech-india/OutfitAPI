using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public class AddressMapper : IAddressMapper
{
    public Address GetEntity(AddressDto addressDto)
    {
        return new Address
        {
            UserId = addressDto.UserId,
            Name = addressDto.Name,
            Line1 = addressDto.Line1,
            Line2 = addressDto.Line2,
            Landmark = addressDto.Landmark,
            Village = addressDto.Village,
            City = addressDto.City,
            District = addressDto.District,
            State = addressDto.State,
            Country = addressDto.Country,
            Pincode = addressDto.Pincode
        };
    }
    public AddressDto GetAddressDto(Address address)
    {
        return new AddressDto
        {
            Id = address.Id,
            UserId = address.UserId.Value,
            Name = address.Name,
            Line1 = address.Line1,
            Line2 = address.Line2,
            Landmark = address.Landmark,
            Village = address.Village,
            City = address.City,
            District = address.District,
            State = address.State,
            Country = address.Country,
            Pincode = address.Pincode
        };
    }
}
