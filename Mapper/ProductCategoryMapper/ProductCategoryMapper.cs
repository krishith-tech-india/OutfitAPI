using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public class ProductCategoryMapper : IProductCategoryMapper
{
    public ProductCategory GetEntity(ProductCategoryDto productCategoryDto)
    {
        return new ProductCategory
        {
            Name = productCategoryDto.Name,
            Description = productCategoryDto.Description
        };
    }

    public ProductCategoryDto GetProductCategoryDto(ProductCategory productCategory)
    {
        return new ProductCategoryDto
        {
            Id = productCategory.Id,
            Name = productCategory.Name,
            Description = productCategory.Description
        };
    }
}
