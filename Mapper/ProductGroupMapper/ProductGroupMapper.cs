using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public class ProductGroupMapper : IProductGroupMapper
{
    public ProductGroup GetEntity(ProductGroupDto productGroupDto)
    {
        return new ProductGroup
        {
            Name = productGroupDto.Name,
            CategoryId = productGroupDto.CategoryId,
            SubTitle = productGroupDto.SubTitle,
            Description = productGroupDto.Description,
            Features = productGroupDto.Features
        };
    }

    public ProductGroupDto GetProductGroupDto(ProductGroup productGroup)
    {
        return new ProductGroupDto
        {
            Id = productGroup.Id,
            Name = productGroup.Name,
            CategoryId = productGroup.CategoryId.Value,
            SubTitle = productGroup.SubTitle,
            Description = productGroup.Description,
            Features = productGroup.Features
        };
    }
}
