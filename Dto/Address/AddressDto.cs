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
    public string UserName { get; set; } = string.Empty;
    public string? AddressName { get; set; } = null;
    public string Line1 { get; set; } = string.Empty;
    public string? Line2 { get; set; } = null;
    public string? Landmark { get; set; } = null;
    public string? Village { get; set; } = null;
    public string? City { get; set; } = null;
    public string District { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;

}
