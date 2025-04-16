using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("images")]
[Index("ImageTypeId", Name = "idx_images_image_type_id")]
[Index("IsDeleted", Name = "idx_images_is_deleted")]
[Index("ProductId", Name = "idx_images_product_id")]
public partial class Image
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("url")]
    [StringLength(1000)]
    public string? Url { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("image_type_id")]
    public int? ImageTypeId { get; set; }

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

    [ForeignKey("AddedBy")]
    [InverseProperty("ImageAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [ForeignKey("ImageTypeId")]
    [InverseProperty("Images")]
    public virtual ImageType? ImageType { get; set; }

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("ImageLastUpdatedByNavigations")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Images")]
    public virtual Product? Product { get; set; }
}
