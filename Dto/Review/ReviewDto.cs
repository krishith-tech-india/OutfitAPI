using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dto;

public class ReviewDto
{
    public int Id { get; set; }
    public int ProductID { get; set; }
    public string? ProductName { get; set; }
    public int Rating { get; set; }
    public string? Review { get; set; }
    [JsonIgnore]
    public int AddedBy { get; set; }
}
