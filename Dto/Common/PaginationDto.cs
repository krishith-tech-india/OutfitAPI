using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class PaginationDto
{
    public int PageSize { get; set; } = 5;
    public int PageNo { get; set; } = 1;
}
