using Core;
using Core.Authentication;
using Data.Contexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public class OrderStatusRepo : BaseRepo<OrderStatus>, IOrderStatusRepo
{
    private readonly IUserContext _userContext;
    public OrderStatusRepo(OutfitDBContext context, IUserContext userContext) : base(context)
    {
        _userContext = userContext;
    }

    public async Task<List<OrderStatus>> GetAllOrderStatusAsync()
    {
        return await Select(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<OrderStatus> GetOrderStatusByIdAsync(int id)
    {
        var orderStatus = await GetByIdAsync(id);
        if (orderStatus == null || orderStatus.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, string.Format(Constants.NotExistExceptionMessage, "Order Status", "Id", id));
        return orderStatus;
    }

    private async Task CheckIsOrderStatusDataValidOrNotAsync(OrderStatus orderStatus)
    {
        if (string.IsNullOrWhiteSpace(orderStatus.Name))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest,string.Format(Constants.FieldrequiredExceptionMessage, "Order Status", "Name"));
        if (await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(orderStatus.Id) && x.Name.ToLower().Equals(orderStatus.Name.ToLower())))
            throw new ApiException(System.Net.HttpStatusCode.Conflict,string.Format(Constants.AleadyExistExceptionMessage, "Order Status", "Name" , orderStatus.Name));
    }

    public async Task InsertOrderStatusAsync(OrderStatus orderStatus)
    {
        await CheckIsOrderStatusDataValidOrNotAsync(orderStatus);
        if (string.IsNullOrWhiteSpace(orderStatus.Description))
            orderStatus.Description = null;
        orderStatus.AddedOn = DateTime.Now;
        orderStatus.AddedBy = _userContext.loggedInUser.Id;
        await InsertAsync(orderStatus);
        await SaveChangesAsync();
    }

    public async Task UpdateOrderStatusAsync(OrderStatus orderStatus)
    {
        await CheckIsOrderStatusDataValidOrNotAsync(orderStatus);
        if (string.IsNullOrWhiteSpace(orderStatus.Description))
            orderStatus.Description = null;
        orderStatus.LastUpdatedOn = DateTime.Now;
        orderStatus.LastUpdatedBy = _userContext.loggedInUser.Id;
        Update(orderStatus);
        await SaveChangesAsync();
    }
}
