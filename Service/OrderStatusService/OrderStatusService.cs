using Core;
using Data.Models;
using Dto;
using Dto.OrderStatus;
using Mapper;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

    public async Task<List<OrderStatusDto>> GetAllOrderStatusAsync(OrderStatusFilterDto orderStatusFilterDto)
    {
        IQueryable<OrderStatus> orderStatusQuery = _orderStatusRepo.GetQueyable().Where(x => !x.IsDeleted);

        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.GenericTextFilter))
            orderStatusQuery = orderStatusQuery.Where(x =>
                        x.Name.ToLower().Contains(orderStatusFilterDto.GenericTextFilter) ||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(orderStatusFilterDto.GenericTextFilter))
                    );

        //FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.NameFilterText))
            orderStatusQuery = orderStatusQuery.Where(x => x.Name.ToLower().Contains(orderStatusFilterDto.NameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.DescriptionFilterText))
            orderStatusQuery = orderStatusQuery.Where(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(orderStatusFilterDto.DescriptionFilterText.ToLower()));

        //OrderByQuery
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.OrderByField) && orderStatusFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            orderStatusQuery = orderStatusQuery.OrderBy(x => x.Name);
        else if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.OrderByField) && orderStatusFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            orderStatusQuery = orderStatusQuery.OrderBy(x => x.Description);
        else
            orderStatusQuery = orderStatusQuery.OrderBy(x => x.Id);

        //Pagination
        if (orderStatusFilterDto.IsPagination)
            orderStatusQuery = orderStatusQuery.Skip((orderStatusFilterDto.PageNo - 1) * orderStatusFilterDto.PageSize).Take(orderStatusFilterDto.PageSize);

        var orderStatus = await orderStatusQuery.ToListAsync();
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

    public async Task<bool> IsOrderStatusExistByNameAsync(string Name)
    {
        return await _orderStatusRepo.CheckIsOrderStatusExistByNameAsync(Name);
    }
}
