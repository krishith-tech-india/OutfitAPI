using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public class CartMapper : ICartMapper
{
    public Cart GetEntity(CartDto cartDto)
    {
        return new Cart
        {
            UserId = cartDto.UserID,
            ProductId = cartDto.ProductID,
            Quantity = cartDto.Quantity,
        };
    }

    public CartDto GetImageTypeDto(Cart cart)
    {
        return new CartDto
        {
            Id = cart.Id,
            UserID = cart.UserId.Value,
            ProductID = cart.ProductId.Value,
            Quantity = cart.Quantity.Value,
        };
    }
}
