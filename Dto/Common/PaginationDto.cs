using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class PaginationDto
{
    private bool isPagination;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public bool IsPagination {
        get
        {
            if (PageSize == 0 || PageNo == 0)
                return false;
            
            return isPagination;
        }
        set 
        {
            isPagination = value;
        } 
    }
}
