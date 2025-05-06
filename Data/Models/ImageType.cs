using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("image_type")]
[Index("IsDeleted", Name = "idx_image_type_is_deleted")]
public partial class ImageType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("description")]
    [StringLength(200)]
    public string? Description { get; set; }

    [Column("added_by")]
    public int? AddedBy { get; set; }

    [Column("added_on", TypeName = "timestamp without time zone")]
    public DateTime? AddedOn { get; set; }

    [Column("last_updated_by")]
    public int? LastUpdatedBy { get; set; }

    [Column("last_updated_on", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdatedOn { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [ForeignKey("AddedBy")]
    [InverseProperty("ImageTypeAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [InverseProperty("ImageType")]
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("ImageTypeLastUpdatedByNavigations")]
    public virtual User? LastUpdatedByNavigation { get; set; }
}
