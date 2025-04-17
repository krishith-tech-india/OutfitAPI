using System.ComponentModel.DataAnnotations;

namespace Dto;

public class RoleDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
