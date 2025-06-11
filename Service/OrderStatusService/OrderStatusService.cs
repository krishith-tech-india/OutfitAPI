using Core;
using Data.Models;
using Dto;
using Dto.Common;
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

    public async Task<PaginatedList<OrderStatusDto>>  GetOrderStatusAsync(OrderStatusFilterDto orderStatusFilterDto)
    {
        // create paginated Address List
        var paginatedorderStatusList = new PaginatedList<OrderStatusDto>();

        //create Predicates
        var orderStatusFilterPredicate = PradicateBuilder.True<OrderStatus>();

        //Apply Order Status is Deleted filter
        orderStatusFilterPredicate = orderStatusFilterPredicate.And(x => !x.IsDeleted);

        //Get Order Status filters
        orderStatusFilterPredicate = ApplyOrderStatusFilters(orderStatusFilterPredicate, orderStatusFilterDto);

        //Apply filters
        IQueryable<OrderStatus> orderStatusQuery = _orderStatusRepo.GetQueyable().Where(orderStatusFilterPredicate);

        //ApplyGenericFilter
        orderStatusQuery = ApplyGenericFilters(orderStatusQuery, orderStatusFilterDto);

        //OrderBy
        orderStatusQuery = ApplyOrderByFilter(orderStatusQuery, orderStatusFilterDto);

        //FatchTotalCount
        paginatedorderStatusList.Count = await orderStatusQuery.CountAsync();

        //Pagination
        orderStatusQuery = ApplyPaginationFilter(orderStatusQuery, orderStatusFilterDto);

        //FatchItems
        paginatedorderStatusList.Items = await orderStatusQuery.Select(x => _orderStatusMapper.GetOrderStatusDto(x)).ToListAsync();

        return paginatedorderStatusList;
    }

    private IQueryable<OrderStatus> ApplyGenericFilters(IQueryable<OrderStatus> orderStatusQuery, OrderStatusFilterDto orderStatusFilterDto)
    {
        //Generic filters
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.GenericTextFilter))
        {
            var genericFilterPredicate = PradicateBuilder.False<OrderStatus>();
            var filterText = orderStatusFilterDto.GenericTextFilter.Trim();
            genericFilterPredicate = genericFilterPredicate
                                    .Or(x => EF.Functions.ILike(x.Name, $"%{filterText}%"))
                                    .Or(x => EF.Functions.ILike(x.Description, $"%{filterText}%"));

            //Apply generic filters
            return orderStatusQuery.Where(genericFilterPredicate);
        }
        return orderStatusQuery;
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

    private Expression<Func<OrderStatus, bool>> ApplyOrderStatusFilters(Expression<Func<OrderStatus, bool>> orderStatusFilterPredicate, OrderStatusFilterDto orderStatusFilterDto)
    {
        //Apply Field Text Filters
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.NameFilterText))
            orderStatusFilterPredicate = orderStatusFilterPredicate.And(x => EF.Functions.ILike(x.Name, $"%{orderStatusFilterDto.NameFilterText.Trim()}%"));
        if (!string.IsNullOrWhiteSpace(orderStatusFilterDto.DescriptionFilterText))
            orderStatusFilterPredicate = orderStatusFilterPredicate.And(x => EF.Functions.ILike(x.Description, $"%{orderStatusFilterDto.DescriptionFilterText.Trim()}%"));

        return orderStatusFilterPredicate;
    }

    private IQueryable<OrderStatus> ApplyOrderByFilter(IQueryable<OrderStatus> orderStatusQuery, OrderStatusFilterDto orderStatusFilterDto)
    {
        var orderByMappings = new Dictionary<string, Expression<Func<OrderStatus, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { Constants.OrderByNameValue, x => x.Name ?? "" },
            { Constants.OrderByDescriptionValue, x => x.Description ?? "" }
        };

        if (!orderByMappings.TryGetValue(orderStatusFilterDto.OrderByField ?? "Id", out var orderByExpression))
        {
            orderByExpression = x => x.Id;
        }

        orderStatusQuery = orderStatusFilterDto.OrderByEnumValue.Equals(OrderByTypeEnum.Desc)
            ? orderStatusQuery.OrderByDescending(orderByExpression)
            : orderStatusQuery.OrderBy(orderByExpression);

        return orderStatusQuery;
    }

    private IQueryable<OrderStatus> ApplyPaginationFilter(IQueryable<OrderStatus> orderStatusQuery, OrderStatusFilterDto orderStatusFilterDto)
    {
        if (orderStatusFilterDto.IsPagination)
            orderStatusQuery = orderStatusQuery.Skip((orderStatusFilterDto.PageNo - 1) * orderStatusFilterDto.PageSize).Take(orderStatusFilterDto.PageSize);

        return orderStatusQuery;
    }
}
