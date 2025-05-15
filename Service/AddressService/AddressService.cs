using Core;
using Core.AppSettingConfigs;
using Data.Models;
using Dto;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class AddressService : IAddressService
{
    private readonly IAddressRepo _addressRepo;
    private readonly IAddressMapper _addressMapper;
    private readonly IUserRepo _userRepo;
    public AddressService(
        IAddressRepo addressRepo,
        IAddressMapper addressMapper,
        IUserRepo userRepo
    )
    {
        _addressRepo = addressRepo;
        _addressMapper = addressMapper;
        _userRepo = userRepo;
    }
    public async Task<AddressDto> GetAddressByIdAsync(int id)
    {
        var addressQuery = _addressRepo.GetQueyable();
        var userQuery = _userRepo.GetQueyable();

        var address = await addressQuery.
            Join(userQuery,
                address => address.UserId,
                user => user.Id,
                (addrss , user) => new
                {
                    addrss.Id,
                    addrss.UserId,
                    UserName = user.Name,
                    addrss.Name,
                    addrss.Line1,
                    addrss.Line2,
                    addrss.Landmark,
                    addrss.Village,
                    addrss.City,
                    addrss.District,
                    addrss.State,
                    addrss.Country,
                    addrss.Pincode,
                    UserIsdeleted = user.IsDeleted,
                }
            ).Where(x => x.Id == id && !x.UserIsdeleted)
            .Select(x => new AddressDto()
            {
                Id = x.Id,
                UserId = x.UserId.Value,
                UserName = x.UserName,
                AddressName = x.Name,
                Line1 = x.Line1,
                Line2 = x.Line2,
                Landmark = x.Landmark,
                Village = x.Village,
                City = x.City,
                District = x.District,
                State = x.State,
                Country = x.Country,
                Pincode = x.Pincode,
            }).FirstOrDefaultAsync();
        if (address == null)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Address", "id", id));
        return address;
    }

    public async Task<List<AddressDto>> GetAddressByUserIdAsync(int UserId, GenericFilterDto genericFilterDto)
    {
        if (!await _userRepo.CheckIsUserIdExistAsync(UserId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound,string.Format(Constants.NotExistExceptionMessage, "User","Id",UserId));

        var addressQuery = _addressRepo.GetQueyable();
        var userQuery = _userRepo.GetQueyable();

        IQueryable<AddressDto> AddressQuery = addressQuery.
            Join(userQuery,
                address => address.UserId,
                user => user.Id,
                (addrss, user) => new
                {
                    addrss.Id,
                    addrss.UserId,
                    UserName = user.Name,
                    addrss.Name,
                    addrss.Line1,
                    addrss.Line2,
                    addrss.Landmark,
                    addrss.Village,
                    addrss.City,
                    addrss.District,
                    addrss.State,
                    addrss.Country,
                    addrss.Pincode,
                }
            ).Where(x => x.UserId == UserId)
            .Select(x => new AddressDto()
            {
                Id = x.Id,
                UserId = x.UserId.Value,
                UserName = x.UserName,
                AddressName = x.Name,
                Line1 = x.Line1,
                Line2 = x.Line2,
                Landmark = x.Landmark,
                Village = x.Village,
                City = x.City,
                District = x.District, 
                State = x.State,
                Country = x.Country,
                Pincode = x.Pincode,
            });

        //TextQuery
        if (!string.IsNullOrWhiteSpace(genericFilterDto.GenericTextFilter))
            AddressQuery = AddressQuery.Where(x =>
                        x.UserName.ToLower().Contains(genericFilterDto.GenericTextFilter) ||
                        (!string.IsNullOrWhiteSpace(x.AddressName) && x.AddressName.ToLower().Contains(genericFilterDto.GenericTextFilter)) ||
                        x.Line1.ToLower().Contains(genericFilterDto.GenericTextFilter) ||
                        (!string.IsNullOrWhiteSpace(x.Line2) && x.Line2.ToLower().Contains(genericFilterDto.GenericTextFilter)) ||
                        (!string.IsNullOrWhiteSpace(x.Landmark) && x.Landmark.ToLower().Contains(genericFilterDto.GenericTextFilter)) ||
                        (!string.IsNullOrWhiteSpace(x.Village) && x.Village.ToLower().Contains(genericFilterDto.GenericTextFilter)) ||
                        x.City.ToLower().Contains(genericFilterDto.GenericTextFilter) ||
                        x.District.ToLower().Contains(genericFilterDto.GenericTextFilter) ||
                        x.State.ToLower().Contains(genericFilterDto.GenericTextFilter) ||
                        x.Country.ToLower().Contains(genericFilterDto.GenericTextFilter) ||
                        x.Pincode.ToLower().Contains(genericFilterDto.GenericTextFilter)
                    );

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByUserNameValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.UserName);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.AddressName);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByLine1Value, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Line1);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByLine2Value, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Line2);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByLandmarkValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Landmark);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByVillageValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Village);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByCityValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.City);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDistrictValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.District);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByStateValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.State);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByCountryValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Country);
        else if (!string.IsNullOrWhiteSpace(genericFilterDto.OrderByField) && genericFilterDto.OrderByField.ToLower().Equals(Constants.OrderByPincodeValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Pincode);
        else
            AddressQuery = AddressQuery.OrderBy(x => x.Id);

        //Pagination
        if (genericFilterDto.IsPagination)
            AddressQuery = AddressQuery.Skip((genericFilterDto.PageNo - 1) * genericFilterDto.PageSize).Take(genericFilterDto.PageSize);

        return await AddressQuery.ToListAsync();
        //var Address = await _addressRepo.GetAddressByUserIdAsync(UserId, genericFilterDto);
        //return Address.Select(x => _addressMapper.GetAddressDto(x)).ToList();
    }

    public async Task AddAddressAsync(AddressDto addressDto)
    {
        if (!await _userRepo.CheckIsUserIdExistAsync(addressDto.UserId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "User", "Id", addressDto.UserId));
        var addressEntity = _addressMapper.GetEntity(addressDto);
        await _addressRepo.InsertUserAsync(addressEntity);
    }


}
