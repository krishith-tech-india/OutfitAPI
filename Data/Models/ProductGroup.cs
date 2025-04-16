using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("product_group")]
[Index("CategoryId", Name = "idx_product_group_category_id")]
[Index("IsDeleted", Name = "idx_product_group_is_deleted")]
public partial class ProductGroup
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("description")]
    [StringLength(5000)]
    public string? Description { get; set; }

    [Column("sub_title")]
    [StringLength(200)]
    public string? SubTitle { get; set; }

    [Column("features")]
    [StringLength(1000)]
    public string? Features { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

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
    [InverseProperty("ProductGroupAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("ProductGroups")]
    public virtual ProductCategory? Category { get; set; }

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("ProductGroupLastUpdatedByNavigations")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [InverseProperty("ProductGroup")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
