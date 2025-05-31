using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public interface IProductCategoryMapper
{
    ProductCategory GetEntity(ProductCategoryDto productCategoryDto);
    ProductCategoryDto GetProductCategoryDto(ProductCategory productCategory);
}
