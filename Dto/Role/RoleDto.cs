using System.ComponentModel.DataAnnotations;

namespace Dto;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? UserCount { get; set; }
    public string? Description { get; set; }
}
