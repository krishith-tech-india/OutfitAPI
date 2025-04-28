using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("review")]
[Index("AddedBy", Name = "idx_review_added_by")]
[Index("IsDeleted", Name = "idx_review_is_deleted")]
[Index("ProductId", Name = "idx_review_product_id")]
[Index("ProductId", "AddedBy", Name = "uk_review_product_id_added_by", IsUnique = true)]
public partial class Review
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("rating")]
    public int? Rating { get; set; }

    [Column("review")]
    [StringLength(1000)]
    public string? Review1 { get; set; }

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
    [InverseProperty("ReviewAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("ReviewLastUpdatedByNavigations")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Reviews")]
    public virtual Product? Product { get; set; }
}
