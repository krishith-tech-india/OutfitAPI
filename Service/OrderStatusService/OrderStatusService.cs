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

    public async Task<List<OrderStatusDto>> GetAllOrderStatusAsync()
    {
        var orderStatus = await _orderStatusRepo.GetAllOrderStatusAsync();
        return orderStatus.Select(x => _orderStatusMapper.GetImageTypeDto(x)).ToList();
    }

    public async Task<OrderStatusDto> GetOrderStatusByIDAsync(int id)
    {
        return _orderStatusMapper.GetImageTypeDto(await _orderStatusRepo.GetOrderStatusByIdAsync(id));
    }

    public async Task InsertOrderStatusAsync(OrderStatusDto orderStatusDto)
    {
        var OrderStatusEntity = _orderStatusMapper.GetEntity(orderStatusDto);
        await _orderStatusRepo.InsertOrderStatusAsync(OrderStatusEntity);
    }
}
