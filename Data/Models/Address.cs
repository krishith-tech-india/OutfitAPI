using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("address")]
[Index("UserId", Name = "idx_address_user_id")]
public partial class Address
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("line1")]
    [StringLength(1000)]
    public string? Line1 { get; set; }

    [Column("line2")]
    [StringLength(1000)]
    public string? Line2 { get; set; }

    [Column("landmark")]
    [StringLength(500)]
    public string? Landmark { get; set; }

    [Column("village")]
    [StringLength(500)]
    public string? Village { get; set; }

    [Column("city")]
    [StringLength(500)]
    public string? City { get; set; }

    [Column("district")]
    [StringLength(500)]
    public string? District { get; set; }

    [Column("state")]
    [StringLength(500)]
    public string? State { get; set; }

    [Column("country")]
    [StringLength(500)]
    public string? Country { get; set; }

    [Column("pincode")]
    [StringLength(20)]
    public string? Pincode { get; set; }

    [Column("added_by")]
    public int? AddedBy { get; set; }

    [Column("added_on", TypeName = "timestamp without time zone")]
    public DateTime? AddedOn { get; set; }

    [ForeignKey("AddedBy")]
    [InverseProperty("AddressAddedByNavigations")]
    public virtual User? AddedByNavigation { get; set; }

    [InverseProperty("Address")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("UserId")]
    [InverseProperty("AddressUsers")]
    public virtual User? User { get; set; }
}
