using Data.Models;
using Dto;
using Dto.OrderStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IOrderStatusRepo : IBaseRepo<OrderStatus>
{
    Task<OrderStatus> GetOrderStatusByIdAsync(int id);
    Task InsertOrderStatusAsync(OrderStatus orderStatus);
    Task UpdateOrderStatusAsync(OrderStatus orderStatus);
    Task<bool> CheckIsOrderStatusExistByNameAsync(string Name);
}
