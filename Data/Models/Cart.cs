using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("cart")]
[Index("IsDeleted", Name = "idx_cart_is_deleted")]
[Index("ProductId", Name = "idx_cart_product_id")]
[Index("UserId", Name = "idx_cart_user_id")]
public partial class Cart
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

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
    [InverseProperty("CartAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("CartLastUpdatedByNavigations")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Carts")]
    public virtual Product? Product { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("CartUsers")]
    public virtual User? User { get; set; }
}
