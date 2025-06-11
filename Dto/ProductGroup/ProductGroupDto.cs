using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class ProductGroupDto
{
    public int Id { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? SubTitle { get; set; }
    public string? Features { get; set; }
}
