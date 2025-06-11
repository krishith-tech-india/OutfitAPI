using Dto;
using Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public interface IProductCategoryService
{
    Task<PaginatedList<ProductCategoryDto>> GetProductCategorysAsync(ProductCategoryFilterDto productCategoryFilterDto);
    Task<ProductCategoryDto> GetProductCategoryByIdAsync(int id);
    Task InsertProductCategoryAsync(ProductCategoryDto productCategoryDto);
    Task UpdateProductCategoryAsync(int id, ProductCategoryDto productCategoryDto);
    Task DeleteProductCategoryAsync(int id);
    Task<bool> IsProductCategoryExistByNameAsync(string Name);

}
