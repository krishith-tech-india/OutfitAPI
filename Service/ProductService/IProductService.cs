using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;
public interface IProductService
{
    Task<List<ProductDto>> GetProductsAsync(ProductFilterDto productFilterDto);
    Task<ProductDto> GetProductByIDAsync(int id);
    Task<List<ProductDto>> GetProductByGroupIdAsync(int id, ProductFilterDto productFilterDto);
    Task InsertProductAsync(ProductDto productDto);
    Task UpdateProductAsync(int id, ProductDto productDto);
    Task DeleteProductAsync(int id);
}
