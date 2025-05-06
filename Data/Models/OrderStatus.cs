using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("order_status")]
[Index("IsDeleted", Name = "idx_order_status_is_deleted")]
public partial class OrderStatus
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
    [InverseProperty("OrderStatusAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("OrderStatusLastUpdatedByNavigations")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();

    [InverseProperty("CurrentStatus")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
