using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class UserFilterDto : GenericFilterDto
{
    public string? RoleNameFilterText { get; set; }
    public string? EmailFilterText { get; set; }
    public string? PhNoFilterText { get; set; }
    public string? NameFilterText { get; set; }
}
