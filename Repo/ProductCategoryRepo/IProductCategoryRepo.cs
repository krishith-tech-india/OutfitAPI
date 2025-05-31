using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IProductCategoryRepo : IBaseRepo<ProductCategory>
{
    Task<ProductCategory> GetProductCategoryByIdAsync(int id);
    Task InsertProductCategoryAsync(ProductCategory productCategory);
    Task UpadteProductCategoryAsync(ProductCategory productCategory);
    Task<bool> IsProductCategoryExistByNameAsync(string Name);
    Task<bool> IsProductCategoryIdExistAsync(int id);
}
