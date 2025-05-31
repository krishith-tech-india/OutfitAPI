using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IProductGroupRepo : IBaseRepo<ProductGroup>
{
    Task<ProductGroup> GetProductGroupByIDAsync(int id);
    Task InsertProductGroupAsync(ProductGroup productGroup);
    Task UpdateProductGroupAsync(ProductGroup productGroup);
    Task<bool> IsProductGroupExistByName(string name);
    Task<bool> IsProductGroupIdExistAsync(int id);
}
