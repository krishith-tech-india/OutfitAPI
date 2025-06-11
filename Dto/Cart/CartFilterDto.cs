using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class CartFilterDto : GenericFilterDto
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? Quantity { get; set; }
}
