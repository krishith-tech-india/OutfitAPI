using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("order_status_history")]
[Index("IsDeleted", Name = "idx_order_status_history_is_deleted")]
[Index("OrderId", Name = "idx_order_status_history_order_id")]
[Index("StatusId", Name = "idx_order_status_history_status_id")]
public partial class OrderStatusHistory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("status_id")]
    public int? StatusId { get; set; }

    [Column("order_id")]
    public int? OrderId { get; set; }

    [Column("change_date", TypeName = "timestamp without time zone")]
    public DateTime? ChangeDate { get; set; }

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
    [InverseProperty("OrderStatusHistoryAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [ForeignKey("LastUpdatedBy")]
    [InverseProperty("OrderStatusHistoryLastUpdatedByNavigations")]
    public virtual User? LastUpdatedByNavigation { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderStatusHistories")]
    public virtual Order? Order { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("OrderStatusHistories")]
    public virtual OrderStatus? Status { get; set; }
}
