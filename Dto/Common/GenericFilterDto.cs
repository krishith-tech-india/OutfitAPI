using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class GenericFilterDto : PaginationDto
{
    public string? GenericTextFilter { get; set; }
    public string? OrderByField { get; set; }
}
