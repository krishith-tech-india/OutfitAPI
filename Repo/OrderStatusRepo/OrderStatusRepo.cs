using Core;
using Data.Contexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public class OrderStatusRepo : BaseRepo<OrderStatus>, IOrderStatusRepo
{
    public OrderStatusRepo(OutfitDBContext context) : base(context)
    {

    }

    public async Task<List<OrderStatus>> GetAllOrderStatusAsync()
    {
        return await Select(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<OrderStatus> GetOrderStatusByIdAsync(int id)
    {
        var orderStatus = await GetByIdAsync(id);
        if (orderStatus == null || orderStatus.IsDeleted)
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"Order Status id {id} not exist");
        return orderStatus;
    }

    public async Task CheckIsImageTypeDataValidOrNotAsync(OrderStatus orderStatus)
    {
        if (string.IsNullOrWhiteSpace(orderStatus.Name))
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, $"Order Status name is required");
        if (await AnyAsync(x => !x.IsDeleted && !x.Id.Equals(orderStatus.Id) && x.Name.ToLower().Equals(orderStatus.Name.ToLower())))
            throw new ApiException(System.Net.HttpStatusCode.Conflict, $"Order Status name {orderStatus.Name} aleady exist");
    }

    public async Task InsertOrderStatusAsync(OrderStatus orderStatus)
    {
        await CheckIsImageTypeDataValidOrNotAsync(orderStatus);
        if (string.IsNullOrWhiteSpace(orderStatus.Description))
            orderStatus.Description = null;
        orderStatus.AddedOn = DateTime.Now;
        //orderStatus.AddedBy = 0;
        await InsertAsync(orderStatus);
        await SaveChangesAsync();
    }
}
