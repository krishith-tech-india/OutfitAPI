using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dto;

public class GenericFilterDto : PaginationDto
{
    public string? OrderByType { get; set; }
    public string? GenericTextFilter { get; set; }
    public string? OrderByField { get; set; }
    public OrderByTypeEnum? OrderByEnumValue { 
        get 
        {
            return !string.IsNullOrEmpty(OrderByType) && (OrderByType.ToLower().Equals("asc") || OrderByType.ToLower().Equals("desc"))
                ? Enum.Parse<OrderByTypeEnum>(OrderByType, true)
                : null;
        } 
    }
}
