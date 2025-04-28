using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("order")]
[Index("AddressId", Name = "idx_order_address_id")]
[Index("IsDeleted", Name = "idx_order_is_deleted")]
[Index("ProductId", Name = "idx_order_product_id")]
[Index("PurchaseDate", Name = "idx_order_purchase_date")]
[Index("UserId", Name = "idx_order_user_id")]
public partial class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("address_id")]
    public int? AddressId { get; set; }

    [Column("purchase_date", TypeName = "timestamp without time zone")]
    public DateTime? PurchaseDate { get; set; }

    [Column("amount")]
    public int? Amount { get; set; }

    [Column("payment_ref_id")]
    [StringLength(100)]
    public string? PaymentRefId { get; set; }

    [Column("current_status_id")]
    public int? CurrentStatusId { get; set; }

    [Column("customer_status_comment")]
    public string? CustomerStatusComment { get; set; }

    [Column("tracking_url")]
    public string? TrackingUrl { get; set; }

    [Column("tracking_identity")]
    [StringLength(1000)]
    public string? TrackingIdentity { get; set; }

    [Column("admin_comment")]
    public string? AdminComment { get; set; }

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
    [InverseProperty("OrderAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Orders")]
    public virtual Address? Address { get; set; }

    [ForeignKey("CurrentStatusId")]
    [InverseProperty("Orders")]
    public virtual OrderStatus? CurrentStatus { get; set; }

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("OrderLastUpdatedByNavigations")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();

    [ForeignKey("ProductId")]
    [InverseProperty("Orders")]
    public virtual Product? Product { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("OrderUsers")]
    public virtual User? User { get; set; }
}
