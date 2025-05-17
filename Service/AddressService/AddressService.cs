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

    public async Task<List<AddressDto>> GetAddressByUserIdAsync(int UserId, AddressFilterDto addressFilterDto)
    {
        if (!await _userRepo.CheckIsUserIdExistAsync(UserId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound,string.Format(Constants.NotExistExceptionMessage, "User","Id",UserId));

        var addressQueriable = _addressRepo.GetQueyable();
        var userQueriable = _userRepo.GetQueyable();

        IQueryable<AddressDto> AddressQuery = addressQueriable.
            Join(userQueriable,
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

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(addressFilterDto.GenericTextFilter))
            AddressQuery = AddressQuery.Where(x =>
                        x.UserName.ToLower().Contains(addressFilterDto.GenericTextFilter) ||
                        (!string.IsNullOrWhiteSpace(x.AddressName) && x.AddressName.ToLower().Contains(addressFilterDto.GenericTextFilter)) ||
                        x.Line1.ToLower().Contains(addressFilterDto.GenericTextFilter) ||
                        (!string.IsNullOrWhiteSpace(x.Line2) && x.Line2.ToLower().Contains(addressFilterDto.GenericTextFilter)) ||
                        (!string.IsNullOrWhiteSpace(x.Landmark) && x.Landmark.ToLower().Contains(addressFilterDto.GenericTextFilter)) ||
                        (!string.IsNullOrWhiteSpace(x.Village) && x.Village.ToLower().Contains(addressFilterDto.GenericTextFilter)) ||
                        x.City.ToLower().Contains(addressFilterDto.GenericTextFilter) ||
                        x.District.ToLower().Contains(addressFilterDto.GenericTextFilter) ||
                        x.State.ToLower().Contains(addressFilterDto.GenericTextFilter) ||
                        x.Country.ToLower().Contains(addressFilterDto.GenericTextFilter) ||
                        x.Pincode.ToLower().Contains(addressFilterDto.GenericTextFilter)
                    );

        //FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(addressFilterDto.UserNameFilterText))
            AddressQuery = AddressQuery.Where(x => x.UserName.ToLower().Contains(addressFilterDto.UserNameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.AddressNameFilterText))
            AddressQuery = AddressQuery.Where(x => !string.IsNullOrWhiteSpace(x.AddressName) && x.AddressName.ToLower().Contains(addressFilterDto.AddressNameFilterText.ToLower()));
        if(!string.IsNullOrWhiteSpace(addressFilterDto.Line1FilterText))
            AddressQuery = AddressQuery.Where(x => x.Line1.ToLower().Contains(addressFilterDto.Line1FilterText.ToLower()));
        if(!string.IsNullOrWhiteSpace(addressFilterDto.Line2FilterText))
            AddressQuery = AddressQuery.Where(x => !string.IsNullOrWhiteSpace(x.Line2) && x.Line2.ToLower().Contains(addressFilterDto.Line2FilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.LandmarkFilterText))
            AddressQuery = AddressQuery.Where(x => !string.IsNullOrWhiteSpace(x.Landmark) && x.Landmark.ToLower().Contains(addressFilterDto.LandmarkFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.VillageFilterText))
            AddressQuery = AddressQuery.Where(x => !string.IsNullOrWhiteSpace(x.Village) && x.Village.ToLower().Contains(addressFilterDto.VillageFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.CityFilterText))
            AddressQuery = AddressQuery.Where(x => x.City.ToLower().Contains(addressFilterDto.CityFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.DistrictFilterText))
            AddressQuery = AddressQuery.Where(x => x.District.ToLower().Contains(addressFilterDto.DistrictFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.StateFilterText))
            AddressQuery = AddressQuery.Where(x => x.State.ToLower().Contains(addressFilterDto.StateFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.CountryFilterText))
            AddressQuery = AddressQuery.Where(x => x.Country.ToLower().Contains(addressFilterDto.CountryFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.PincodeFilterText))
            AddressQuery = AddressQuery.Where(x => x.Pincode.ToLower().Contains(addressFilterDto.PincodeFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByUserNameValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.UserName);
        else if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.AddressName);
        else if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByLine1Value, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Line1);
        else if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByLine2Value, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Line2);
        else if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByLandmarkValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Landmark);
        else if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByVillageValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Village);
        else if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByCityValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.City);
        else if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDistrictValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.District);
        else if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByStateValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.State);
        else if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByCountryValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Country);
        else if (!string.IsNullOrWhiteSpace(addressFilterDto.OrderByField) && addressFilterDto.OrderByField.ToLower().Equals(Constants.OrderByPincodeValue, StringComparison.OrdinalIgnoreCase))
            AddressQuery = AddressQuery.OrderBy(x => x.Pincode);
        else
            AddressQuery = AddressQuery.OrderBy(x => x.Id);

        //Pagination
        if (addressFilterDto.IsPagination)
            AddressQuery = AddressQuery.Skip((addressFilterDto.PageNo - 1) * addressFilterDto.PageSize).Take(addressFilterDto.PageSize);

        return await AddressQuery.ToListAsync();
    }

    public async Task AddAddressAsync(AddressDto addressDto)
    {
        if (!await _userRepo.CheckIsUserIdExistAsync(addressDto.UserId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "User", "Id", addressDto.UserId));
        var addressEntity = _addressMapper.GetEntity(addressDto);
        await _addressRepo.InsertUserAsync(addressEntity);
    }


}
