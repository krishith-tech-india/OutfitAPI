﻿using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface ICartRepo : IBaseRepo<Cart>
{
    Task InsertCartAsync(Cart cart);
    Task UpdateCartAsync(Cart cart);
    Task<Cart> GetCartByIDAsync(int id);
}
