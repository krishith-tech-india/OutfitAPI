using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class AddressDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? AddressName { get; set; } = null;
    public string? Line1 { get; set; } = null;
    public string? Line2 { get; set; } = null;
    public string? Landmark { get; set; } = null;
    public string? Village { get; set; } = null;
    public string? City { get; set; } = null;
    public string? District { get; set; } = null;
    public string? State { get; set; } = null;
    public string? Country { get; set; } = null;
    public string? Pincode { get; set; } = null;

}
