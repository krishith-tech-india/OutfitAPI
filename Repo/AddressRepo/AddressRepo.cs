using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public class AddressRepo : BaseRepo<Address>, IAddressRepo
{
    private readonly IUserContext _userContext;

    public AddressRepo(OutfitDBContext context, IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }

    private void CheckDataValidOrnot(Address address)
    {
        if (string.IsNullOrWhiteSpace(address.Line1))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage,"Address", "Line 1"));
        if (string.IsNullOrWhiteSpace(address.City))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Address", "City"));
        if (string.IsNullOrWhiteSpace(address.District))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Address", "District"));
        if (string.IsNullOrWhiteSpace(address.State))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Address", "State"));
        if (string.IsNullOrWhiteSpace(address.Country))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Address", "Country"));
        if (string.IsNullOrWhiteSpace(address.Pincode))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, string.Format(Constants.FieldrequiredExceptionMessage, "Address", "Pincode"));
        if (string.IsNullOrWhiteSpace(address.Name))
            address.Name = null;
        if (string.IsNullOrWhiteSpace(address.Line2))
            address.Line2 = null;
        if (string.IsNullOrWhiteSpace(address.Landmark))
            address.Landmark = null;
        if (string.IsNullOrWhiteSpace(address.Village))
            address.Village = null;
    }

    public async Task<Address> GetAddressByIdAsync(int id)
    {
        var address = await GetByIdAsync(id);
        if (address == null)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Address", "id", id));
        return address;
    }

    public async Task<List<Address>> GetAddressByUserIdAsync(int userId, PaginationDto paginationDto)
    {
        IQueryable<Address> AddressQuery = GetQueyable().Where(x => x.UserId.Equals(userId)).OrderBy(x => x.Id);
        
        if (paginationDto.IsPagination)
            AddressQuery = AddressQuery.Skip((paginationDto.PageNo - 1) * paginationDto.PageSize).Take(paginationDto.PageSize);
        
        return await AddressQuery.ToListAsync();
    }

    public async Task InsertUserAsync(Address address)
    {
        CheckDataValidOrnot(address);
        address.AddedOn = DateTime.Now;
        address.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(address);
        await SaveChangesAsync();
    }

}
