using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class CartDto
{
    public int Id { get; set; }
    public int UserID { get; set; }
    public string? UserName { get; set; }
    public int ProductID { get; set; }
    public string? ProductName { get; set; }
    public int? Quantity { get; set; }
}
