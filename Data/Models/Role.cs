using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("role")]
[Index("IsDeleted", Name = "idx_role_is_deleted")]
public partial class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("role_name")]
    [StringLength(20)]
    public string RoleName { get; set; } = null!;

    [Column("role_desc")]
    [StringLength(100)]
    public string? RoleDesc { get; set; }

    [Column("added_by")]
    public int? AddedBy { get; set; }

    [Column("added_on", TypeName = "timestamp without time zone")]
    public DateTime? AddedOn { get; set; }

    [Column("last_updated_by")]
    public int? LastUpdatedBy { get; set; }

    [Column("last_updated_on", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdatedOn { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
