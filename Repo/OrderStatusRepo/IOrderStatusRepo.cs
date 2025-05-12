using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IOrderStatusRepo : IBaseRepo<OrderStatus>
{
    Task<List<OrderStatus>> GetAllOrderStatusAsync(PaginationDto paginationDto);
    Task<OrderStatus> GetOrderStatusByIdAsync(int id);
    Task InsertOrderStatusAsync(OrderStatus orderStatus);
    Task UpdateOrderStatusAsync(OrderStatus orderStatus);
}
