using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dto;

public class ImageDto
{
    public int Id { get; set; }
    [Required]
    public string Url { get; set; } = null!;
    [JsonIgnore]
    public int ProductId { get; set; }
    public int ImageTypeId { get; set; }
    public int Priority { get; set; }
}
