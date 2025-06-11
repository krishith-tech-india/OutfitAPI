using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IProductRepo : IBaseRepo<Product>
{
    Task<int> InsertProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task<Product> GetProductByIDAsync(int id);
    Task<bool> IsProductIdExistAsync(int id);
}
