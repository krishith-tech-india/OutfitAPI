using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class ReviewFilterDto : GenericFilterDto
{
    public string? ProductNameFilterText { get; set; }
    public string? RatingFilterText { get; set; }
    public string? ReviewFilterText { get; set; }
}
