using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [Required]
    public List<ImageDto> Images { get; set; } = new List<ImageDto>();
    [Required]
    public int OriginalPrice { get; set; }
    [Required]
    public int DiscountedPrice { get; set; } 
    public string? SubTitle { get; set; }
    public int? Strach { get; set; }
    public int? Softness { get; set; }
    public int? Transparency { get; set; }
    public string? Fabric { get; set; }
    public string? Color { get; set; } = null!;
    public string? Print { get; set; }
    public int? Size { get; set; }
    public string? Features { get; set; }
    public string? Length { get; set; }
    [Required]
    public int DeliveryPrice { get; set; }
    public string? WashingInstruction { get; set; }
    public string? IroningInstruction { get; set; }
    public string? BleachingInstruction { get; set; }
    public string? DryingInstruction { get; set; }
    public int ProductGroupId { get; set; }
    public string? ProductGroupName { get; set; }
}
