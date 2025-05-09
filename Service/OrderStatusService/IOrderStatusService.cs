using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IOrderStatusService
{
    Task<OrderStatusDto> GetOrderStatusByIDAsync(int id);
    Task<List<OrderStatusDto>> GetAllOrderStatusAsync();
    Task InsertOrderStatusAsync(OrderStatusDto orderStatusDto);
    Task UpdateOrderStatusAsync(int id, OrderStatusDto orderStatusDto);
    Task DeleteOrderStatusAsync(int id);
}
