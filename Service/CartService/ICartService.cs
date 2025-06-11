using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface ICartService
{
    Task<List<CartDto>> GetCartsAsync(CartFilterDto cartFilterDto);
    Task InsertCartAsync(CartDto cartDto);
    Task UpdateCartAsync(int id, CartDto cartDto);
    Task DeleteCartAsync(int id);
}
