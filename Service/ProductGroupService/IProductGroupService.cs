using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IProductGroupService
{
    Task<List<ProductGroupDto>> GetProductGroupsAsync(ProductGroupFilterDto productGroupFilterDto);
    Task<ProductGroupDto> GetProductGroupByIDAsync(int id);
    Task<List<ProductGroupDto>> GetProductGroupByCategoryIdAsync(int id, ProductGroupFilterDto productGroupFilterDto);
    Task InsertProductGroupAsync(ProductGroupDto productGroupDto);
    Task UpdateProductGroupAsync(int id, ProductGroupDto productGroupDto);
    Task DeleteProductGroupAsync(int id);
    Task<bool> IsProductGroupExistByName(string name);
}
