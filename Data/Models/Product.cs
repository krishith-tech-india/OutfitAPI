using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("product")]
[Index("IsDeleted", Name = "idx_product_is_deleted")]
[Index("ProductGroupId", Name = "idx_product_product_group_id")]
public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("description")]
    [StringLength(5000)]
    public string? Description { get; set; }

    [Column("original_price")]
    public int? OriginalPrice { get; set; }

    [Column("discounted_price")]
    public int? DiscountedPrice { get; set; }

    [Column("sub_title")]
    [StringLength(100)]
    public string? SubTitle { get; set; }

    [Column("strach")]
    public int? Strach { get; set; }

    [Column("softness")]
    public int? Softness { get; set; }

    [Column("transparency")]
    public int? Transparency { get; set; }

    [Column("fabric")]
    [StringLength(100)]
    public string? Fabric { get; set; }

    [Column("color")]
    [StringLength(100)]
    public string? Color { get; set; }

    [Column("print")]
    [StringLength(100)]
    public string? Print { get; set; }

    [Column("size")]
    public int? Size { get; set; }

    [Column("features")]
    [StringLength(100)]
    public string? Features { get; set; }

    [Column("length")]
    [StringLength(100)]
    public string? Length { get; set; }

    [Column("delivery_price")]
    public int? DeliveryPrice { get; set; }

    [Column("washing_instruction")]
    [StringLength(1000)]
    public string? WashingInstruction { get; set; }

    [Column("ironing_instruction")]
    [StringLength(1000)]
    public string? IroningInstruction { get; set; }

    [Column("bleaching_instruction")]
    [StringLength(1000)]
    public string? BleachingInstruction { get; set; }

    [Column("drying_instruction")]
    [StringLength(1000)]
    public string? DryingInstruction { get; set; }

    [Column("product_group_id")]
    public int? ProductGroupId { get; set; }

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
    [InverseProperty("ProductAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("Product")]
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("ProductLastUpdatedByNavigations")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Product")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("ProductGroupId")]
    [InverseProperty("Products")]
    public virtual ProductGroup? ProductGroup { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Product")]
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
