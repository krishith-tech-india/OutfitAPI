using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public interface IProductGroupMapper
{
    ProductGroup GetEntity(ProductGroupDto productGroupDto);
    ProductGroupDto GetProductGroupDto(ProductGroup productGroup);
}
