using Core;
using Data.Contexts;
using Data.Models;
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
    public AddressRepo(OutfitDBContext context) : base(context)
    {
    }

    public void CheckDataValidOrnot(Address address)
    {
        if (string.IsNullOrWhiteSpace(address.Line1))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"Address Line 1 is required");
        if (string.IsNullOrWhiteSpace(address.City))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"City is required");
        if (string.IsNullOrWhiteSpace(address.State))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"State is required");
        if (string.IsNullOrWhiteSpace(address.Country))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"Country is required");
        if (string.IsNullOrWhiteSpace(address.Pincode))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"Pincode is required");
        if (string.IsNullOrWhiteSpace(address.Name))
            address.Name = null;
        if (string.IsNullOrWhiteSpace(address.Line2))
            address.Line2 = null;
        if (string.IsNullOrWhiteSpace(address.Landmark))
            address.Landmark = null;
        if (string.IsNullOrWhiteSpace(address.Village))
            address.Village = null;
        if (string.IsNullOrWhiteSpace(address.District))
            address.District = null;
    }

    public async Task<Address> GetAddressByIdAsync(int id)
    {
        var address = await GetByIdAsync(id);
        if (address == null)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Address id {id} not exist");
        return address;
    }

    public async Task<List<Address>> GetAddressByUserIdAsync(int userId)
    {
        List<Address> addresses = await Select(x => x.UserId.Equals(userId)).ToListAsync();
        if (addresses.Count == 0)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"In {userId} UserId Address is Not exist");
        return addresses;
    }

    public async Task InsertUserAsync(Address address)
    {
        CheckDataValidOrnot(address);
        address.AddedOn = DateTime.Now;
        //address.AddedBy = 0;
        await InsertAsync(address);
        await SaveChangesAsync();
    }

}
