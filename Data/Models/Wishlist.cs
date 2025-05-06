using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("wishlist")]
[Index("IsDeleted", Name = "idx_wishlist_is_deleted")]
[Index("ProductId", Name = "idx_wishlist_product_id")]
[Index("UserId", Name = "idx_wishlist_user_id")]
[Index("ProductId", "UserId", Name = "uk_review_product_id_user_id", IsUnique = true)]
public partial class Wishlist
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

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
    [InverseProperty("WishlistAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("WishlistLastUpdatedByNavigations")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Wishlists")]
    public virtual Product? Product { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("WishlistUsers")]
    public virtual User? User { get; set; }
}
