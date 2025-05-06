using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("user")]
[Index("Email", Name = "idx_user_email")]
[Index("IsDeleted", Name = "idx_user_is_deleted")]
[Index("PhNo", Name = "idx_user_ph_no")]
[Index("RoleId", Name = "idx_user_role_id")]
[Index("Email", Name = "uk_user_email_key", IsUnique = true)]
[Index("PhNo", Name = "uk_user_ph_no_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("ph_no")]
    [StringLength(20)]
    public string PhNo { get; set; } = null!;

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

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
    [InverseProperty("InverseAddedByNavigation")]
    public virtual User? AddedByNavigation { get; set; }

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<Address> AddressAddedByNavigations { get; set; } = new List<Address>();

    [InverseProperty("User")]
    public virtual ICollection<Address> AddressUsers { get; set; } = new List<Address>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<Cart> CartAddedByNavigations { get; set; } = new List<Cart>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<Cart> CartLastUpdatedByNavigations { get; set; } = new List<Cart>();

    [InverseProperty("User")]
    public virtual ICollection<Cart> CartUsers { get; set; } = new List<Cart>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<Image> ImageAddedByNavigations { get; set; } = new List<Image>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<Image> ImageLastUpdatedByNavigations { get; set; } = new List<Image>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<ImageType> ImageTypeAddedByNavigations { get; set; } = new List<ImageType>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<ImageType> ImageTypeLastUpdatedByNavigations { get; set; } = new List<ImageType>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<User> InverseAddedByNavigation { get; set; } = new List<User>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<User> InverseLastUpdatedByNavigation { get; set; } = new List<User>();

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("InverseLastUpdatedByNavigation")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<Order> OrderAddedByNavigations { get; set; } = new List<Order>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<OrderItem> OrderItemAddedByNavigations { get; set; } = new List<OrderItem>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<OrderItem> OrderItemLastUpdatedByNavigations { get; set; } = new List<OrderItem>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<Order> OrderLastUpdatedByNavigations { get; set; } = new List<Order>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<OrderStatus> OrderStatusAddedByNavigations { get; set; } = new List<OrderStatus>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<OrderStatusHistory> OrderStatusHistoryAddedByNavigations { get; set; } = new List<OrderStatusHistory>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<OrderStatusHistory> OrderStatusHistoryLastUpdatedByNavigations { get; set; } = new List<OrderStatusHistory>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<OrderStatus> OrderStatusLastUpdatedByNavigations { get; set; } = new List<OrderStatus>();

    [InverseProperty("User")]
    public virtual ICollection<Order> OrderUsers { get; set; } = new List<Order>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<Product> ProductAddedByNavigations { get; set; } = new List<Product>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<ProductCategory> ProductCategoryAddedByNavigations { get; set; } = new List<ProductCategory>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<ProductCategory> ProductCategoryLastUpdatedByNavigations { get; set; } = new List<ProductCategory>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<ProductGroup> ProductGroupAddedByNavigations { get; set; } = new List<ProductGroup>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<ProductGroup> ProductGroupLastUpdatedByNavigations { get; set; } = new List<ProductGroup>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<Product> ProductLastUpdatedByNavigations { get; set; } = new List<Product>();

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<Review> ReviewAddedByNavigations { get; set; } = new List<Review>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<Review> ReviewLastUpdatedByNavigations { get; set; } = new List<Review>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("AddedByNavigation")]
    public virtual ICollection<Wishlist> WishlistAddedByNavigations { get; set; } = new List<Wishlist>();

    [InverseProperty("LastUpdatedByNavigation")]
    public virtual ICollection<Wishlist> WishlistLastUpdatedByNavigations { get; set; } = new List<Wishlist>();

    [InverseProperty("User")]
    public virtual ICollection<Wishlist> WishlistUsers { get; set; } = new List<Wishlist>();
}
