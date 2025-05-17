using Dto;
using Dto.OrderStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IOrderStatusService
{
    Task<List<OrderStatusDto>> GetAllOrderStatusAsync(OrderStatusFilterDto genericFilterDto);
    Task<OrderStatusDto> GetOrderStatusByIDAsync(int id);
    Task InsertOrderStatusAsync(OrderStatusDto orderStatusDto);
    Task UpdateOrderStatusAsync(int id, OrderStatusDto orderStatusDto);
    Task DeleteOrderStatusAsync(int id);
    Task<bool> IsOrderStatusExistByNameAsync(string Name);
}
