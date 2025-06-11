using Dto;
using Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IProductGroupService
{
    Task<PaginatedList<ProductGroupDto>> GetProductGroupsAsync(ProductGroupFilterDto productGroupFilterDto);
    Task<ProductGroupDto> GetProductGroupByIDAsync(int id);
    Task<PaginatedList<ProductGroupDto>> GetProductGroupByCategoryIdAsync(int id, ProductGroupFilterDto productGroupFilterDto);
    Task InsertProductGroupAsync(ProductGroupDto productGroupDto);
    Task UpdateProductGroupAsync(int id, ProductGroupDto productGroupDto);
    Task DeleteProductGroupAsync(int id);
    Task<bool> IsProductGroupExistByName(string name);
}
