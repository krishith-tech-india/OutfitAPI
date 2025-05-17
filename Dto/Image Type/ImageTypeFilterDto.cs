using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class ImageTypeFilterDto : GenericFilterDto
{
    public string? NameFilterText { get; set; }
    public string? DescriptionFilterText { get; set; }
}
