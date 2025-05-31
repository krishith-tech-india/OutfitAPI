using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class ProductGroupFilterDto : GenericFilterDto
{
    public string? NameFilterText { get; set; }
    public string? DescriptionFilterText { get; set; }
    public string? SubTitleFilterText { get; set; }
    public string? FeaturesFilterText { get; set; }
    public string? CategoryNameFilterText { get; set; }
}
