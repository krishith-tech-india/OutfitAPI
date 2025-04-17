using System.ComponentModel.DataAnnotations;

namespace Dto;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? AddedOn { get; set; }
}
