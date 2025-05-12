using Dto;
using Mapper;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class OrderStatusService : IOrderStatusService
{
    private readonly IOrderStatusRepo _orderStatusRepo;
    private readonly IOrderStatusMapper _orderStatusMapper;

    public OrderStatusService(
        IOrderStatusRepo orderStatusRepo,
        IOrderStatusMapper orderStatusMapper
    )
    {
        _orderStatusRepo = orderStatusRepo;
        _orderStatusMapper = orderStatusMapper;
    }

    public async Task<List<OrderStatusDto>> GetAllOrderStatusAsync(PaginationDto paginationDto)
    {
        var orderStatus = await _orderStatusRepo.GetAllOrderStatusAsync(paginationDto);
        return orderStatus.Select(x => _orderStatusMapper.GetOrderStatusDto(x)).ToList();
    }

    public async Task<OrderStatusDto> GetOrderStatusByIDAsync(int id)
    {
        return _orderStatusMapper.GetOrderStatusDto(await _orderStatusRepo.GetOrderStatusByIdAsync(id));
    }

    public async Task InsertOrderStatusAsync(OrderStatusDto orderStatusDto)
    {
        var OrderStatusEntity = _orderStatusMapper.GetEntity(orderStatusDto);
        await _orderStatusRepo.InsertOrderStatusAsync(OrderStatusEntity);
    }

    public async Task UpdateOrderStatusAsync(int id, OrderStatusDto orderStatusDto)
    {
        var OrderStatus = await _orderStatusRepo.GetOrderStatusByIdAsync(id);
        OrderStatus.Name = orderStatusDto.Name;
        OrderStatus.Description = orderStatusDto.Description;
        await _orderStatusRepo.UpdateOrderStatusAsync(OrderStatus);
    }

    public async Task DeleteOrderStatusAsync(int id)
    {
        var OrderStatus = await _orderStatusRepo.GetOrderStatusByIdAsync(id);
        OrderStatus.IsDeleted = true;
        await _orderStatusRepo.UpdateOrderStatusAsync(OrderStatus);
    }
}
