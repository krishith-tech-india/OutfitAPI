using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dto;

public class PaginationDto
{
    public int PageSize { get; set; }
    public int PageNo { get; set; }

    [JsonIgnore]
    public bool IsPagination
    {
        get
        {
            if (PageSize == 0 || PageNo == 0)
                return false;

            return true;
        }
    }
}
