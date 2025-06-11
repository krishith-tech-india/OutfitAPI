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
            Name = addressDto.AddressName,
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
}
