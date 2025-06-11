using Core;
using Core.AppSettingConfigs;
using Data.Models;
using Dto;
using Dto.Common;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        var addressQuery = _addressRepo.GetQueyable().Where(x => x.Id == id);
        var userQuery = _userRepo.GetQueyable().Where(x => !x.IsDeleted);

        var address = await addressQuery.
            Join(userQuery,
                address => address.UserId,
                user => user.Id,
                (address, user) => new AddressDto()
                {
                    Id = address.Id,
                    UserId = address.UserId.Value,
                    UserName = user.Name,
                    AddressName = address.Name,
                    Line1 = address.Line1,
                    Line2 = address.Line2,
                    Landmark = address.Landmark,
                    Village = address.Village,
                    City = address.City,
                    District = address.District,
                    State = address.State,
                    Country = address.Country,
                    Pincode = address.Pincode,
                }
            ).FirstOrDefaultAsync();

        if (address == null)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Address", "id", id));
        return address;
    }

    public async Task<PaginatedList<AddressDto>> GetAddressByUserIdAsync(int userId, AddressFilterDto addressFilterDto)
    {
        if (!await _userRepo.IsUserIdExistAsync(userId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "User", "Id", userId));

        // create paginated Address List
        var paginatedAddressList = new PaginatedList<AddressDto>();

        //create Predicates
        var addressFilterPredicate = PradicateBuilder.True<Address>();
        var userFilterPredicate = PradicateBuilder.True<User>();

        //Apply user id filter
        addressFilterPredicate = addressFilterPredicate.And(x => x.UserId.Equals(userId));

        //Get address filters
        addressFilterPredicate = ApplyAddressFilters(addressFilterPredicate, addressFilterDto);

        //Get user filters
        userFilterPredicate = ApplyUserFilters(userFilterPredicate, addressFilterDto);

        //Apply filters
        var addressQueriable = _addressRepo.GetQueyable().Where(addressFilterPredicate);
        var userQueriable = _userRepo.GetQueyable().Where(userFilterPredicate);

        //join
        IQueryable<AddressDto> addressQuery = addressQueriable.
            Join(userQueriable,
                address => address.UserId,
                user => user.Id,
                (addrss, user) => new AddressDto()
                {
                    Id = addrss.Id,
                    UserId = addrss.UserId.Value,
                    UserName = user.Name,
                    AddressName = addrss.Name,
                    Line1 = addrss.Line1,
                    Line2 = addrss.Line2,
                    Landmark = addrss.Landmark,
                    Village = addrss.Village,
                    City = addrss.City,
                    District = addrss.District,
                    State = addrss.State,
                    Country = addrss.Country,
                    Pincode = addrss.Pincode,
                }
            );

        //ApplyGenericFilter
        addressQuery = ApplyGenericFilters(addressQuery, addressFilterDto);

        //OrderBy
        addressQuery = ApplyOrderByFilter(addressQuery, addressFilterDto);
        
        //FatchTotalCount
        paginatedAddressList.Count = await addressQuery.CountAsync();

        //Pagination
        addressQuery = ApplyPaginationFilter(addressQuery, addressFilterDto);

        //FatchItems
        paginatedAddressList.Items = await addressQuery.ToListAsync();

        return paginatedAddressList;
    }

    public async Task InsertAddressAsync(AddressDto addressDto)
    {
        if (!await _userRepo.IsUserIdExistAsync(addressDto.UserId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "User", "Id", addressDto.UserId));
        var addressEntity = _addressMapper.GetEntity(addressDto);
        await _addressRepo.InsertAddressAsync(addressEntity);
    }

    private IQueryable<AddressDto> ApplyGenericFilters(IQueryable<AddressDto> addressQuery, AddressFilterDto addressFilterDto)
    {
        //Generic filters
        if (!string.IsNullOrWhiteSpace(addressFilterDto.GenericTextFilter))
        {
            var genericFilterPredicate = PradicateBuilder.False<AddressDto>();
            var filterText = addressFilterDto.GenericTextFilter.Trim();
            genericFilterPredicate = genericFilterPredicate
                                    .Or(x => EF.Functions.ILike(x.AddressName, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Line1, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Line2, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Landmark, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Village, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.City, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.District, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.State, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Country, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Pincode, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.UserName, $"%{filterText}%"));

            //Apply generic filters
            return addressQuery.Where(genericFilterPredicate);
        }

        return addressQuery;
    }

    private Expression<Func<Address, bool>> ApplyAddressFilters(Expression<Func<Address, bool>> addressFilterPredicate, AddressFilterDto addressFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(addressFilterDto.AddressNameFilterText))
            addressFilterPredicate = addressFilterPredicate.And(x => EF.Functions.ILike(x.Name, $"%{addressFilterDto.AddressNameFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.Line1FilterText))
            addressFilterPredicate = addressFilterPredicate.And(x => EF.Functions.ILike(x.Line1, $"%{addressFilterDto.Line1FilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.Line2FilterText))
            addressFilterPredicate = addressFilterPredicate.And(x => EF.Functions.ILike(x.Line2, $"%{addressFilterDto.Line2FilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.LandmarkFilterText))
            addressFilterPredicate = addressFilterPredicate.And(x => EF.Functions.ILike(x.Landmark, $"%{addressFilterDto.LandmarkFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.VillageFilterText))
            addressFilterPredicate = addressFilterPredicate.And(x => EF.Functions.ILike(x.Village, $"%{addressFilterDto.VillageFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.CityFilterText))
            addressFilterPredicate = addressFilterPredicate.And(x => EF.Functions.ILike(x.City, $"%{addressFilterDto.CityFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.DistrictFilterText))
            addressFilterPredicate = addressFilterPredicate.And(x => EF.Functions.ILike(x.District, $"%{addressFilterDto.DistrictFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.StateFilterText))
            addressFilterPredicate = addressFilterPredicate.And(x => EF.Functions.ILike(x.State, $"%{addressFilterDto.StateFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.CountryFilterText))
            addressFilterPredicate = addressFilterPredicate.And(x => EF.Functions.ILike(x.Country, $"%{addressFilterDto.CountryFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(addressFilterDto.PincodeFilterText))
            addressFilterPredicate = addressFilterPredicate.And(x => EF.Functions.ILike(x.Pincode, $"%{addressFilterDto.PincodeFilterText.Trim()}%"));

        return addressFilterPredicate;
    }

    private Expression<Func<User, bool>> ApplyUserFilters(Expression<Func<User, bool>> userFilterPredicate, AddressFilterDto addressFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(addressFilterDto.UserNameFilterText))
            userFilterPredicate = userFilterPredicate.And(x => EF.Functions.ILike(x.Name, $"%{addressFilterDto.UserNameFilterText.Trim()}%"));

        return userFilterPredicate;
    }

    private IQueryable<AddressDto> ApplyOrderByFilter(IQueryable<AddressDto> addressQuery, AddressFilterDto addressFilterDto)
    {
        var orderByMappings = new Dictionary<string, Expression<Func<AddressDto, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.OrderByUserNameValue, x => x.UserName ?? "" },
            { Constants.OrderByNameValue, x => x.AddressName ?? "" },
            { Constants.OrderByLine1Value, x => x.Line1 ?? "" },
            { Constants.OrderByLine2Value, x => x.Line2 ?? "" },
            { Constants.OrderByLandmarkValue, x => x.Landmark ?? "" },
            { Constants.OrderByVillageValue, x => x.Village ?? "" },
            { Constants.OrderByCityValue, x => x.City ?? "" },
            { Constants.OrderByDistrictValue, x => x.District ?? "" },
            { Constants.OrderByStateValue, x => x.State ?? "" },
            { Constants.OrderByCountryValue, x => x.Country ?? "" },
            { Constants.OrderByPincodeValue, x => x.Pincode ?? "" }
        };

        if (!orderByMappings.TryGetValue(addressFilterDto.OrderByField ?? "Id", out var orderByExpression))
        {
            orderByExpression = x => x.Id;
        }

        addressQuery = addressFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Desc)
            ? addressQuery.OrderByDescending(orderByExpression)
            : addressQuery.OrderBy(orderByExpression);

        return addressQuery;
    }

    private IQueryable<AddressDto> ApplyPaginationFilter(IQueryable<AddressDto> addressQuery, AddressFilterDto addressFilterDto)
    {
        if (addressFilterDto.IsPagination)
            addressQuery = addressQuery.Skip((addressFilterDto.PageNo - 1) * addressFilterDto.PageSize).Take(addressFilterDto.PageSize);

        return addressQuery;
    }
}
