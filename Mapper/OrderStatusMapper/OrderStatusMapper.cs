using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public class OrderStatusMapper : IOrderStatusMapper
{
    public OrderStatus GetEntity(OrderStatusDto orderStatusDto)
    {
        return new OrderStatus
        {
            Name = orderStatusDto.Name,
            Description = orderStatusDto.Description
        };
    }

    public OrderStatusDto GetOrderStatusDto(OrderStatus orderStatus)
    {
        return new OrderStatusDto
        {
            Id = orderStatus.Id,
            Name = orderStatus.Name,
            Description = orderStatus.Description
        };
    }
}
