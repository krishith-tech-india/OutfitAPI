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
using System.Linq.Expressions;
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

    public async Task<List<OrderStatusDto>>  GetOrderStatusAsync(OrderStatusFilterDto orderStatusFilterDto)
    {
        IQueryable<OrderStatus> orderStatusQuery = _orderStatusRepo.GetQueyable();
        var orderStatusFilter = PradicateBuilder.True<OrderStatus>().And(x => !x.IsDeleted);
        //GenericTextFilterQuery
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.GenericTextFilter))

            orderStatusFilter = orderStatusFilter.And(x =>
                        x.Name.ToLower().Contains(orderStatusFilterDto.GenericTextFilter.ToLower()) ||
                        (!string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(orderStatusFilterDto.GenericTextFilter.ToLower()))
                    );

        //FieldTextFilterQuery
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.NameFilterText))
            orderStatusFilter = orderStatusFilter.And(x => x.Name.ToLower().Contains(orderStatusFilterDto.NameFilterText.ToLower()));
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.DescriptionFilterText))
            orderStatusFilter = orderStatusFilter.And(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(orderStatusFilterDto.DescriptionFilterText.ToLower()));

        orderStatusQuery = orderStatusQuery.Where(orderStatusFilter);

        //OrderByQuery
        Expression<Func<OrderStatus, object>> orderByExpression;
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.OrderByField) && orderStatusFilterDto.OrderByField.ToLower().Equals(Constants.OrderByNameValue, StringComparison.OrdinalIgnoreCase))
            orderByExpression = orderStatus => orderStatus.Name ?? "";
        else if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.OrderByField) && orderStatusFilterDto.OrderByField.ToLower().Equals(Constants.OrderByDescriptionValue, StringComparison.OrdinalIgnoreCase))
            orderByExpression = orderStatus => orderStatus.Description ?? "";
        else
            orderByExpression = orderStatus => orderStatus.Id;

        orderStatusQuery = 
            orderStatusFilterDto.OrderByEnumValue == null || orderStatusFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Asc)
            ? orderStatusQuery.OrderBy(orderByExpression)
            : orderStatusQuery.OrderByDescending(orderByExpression);

        //Pagination
        if (orderStatusFilterDto.IsPagination)
            orderStatusQuery = orderStatusQuery.Skip((orderStatusFilterDto.PageNo - 1) * orderStatusFilterDto.PageSize).Take(orderStatusFilterDto.PageSize);

        return await orderStatusQuery.Select(x => _orderStatusMapper.GetOrderStatusDto(x)).ToListAsync();
    }

    public async Task<OrderStatusDto> GetOrderStatusByIdAsync(int id)
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
        return await _orderStatusRepo.IsOrderStatusExistByNameAsync(Name);
    }
}
