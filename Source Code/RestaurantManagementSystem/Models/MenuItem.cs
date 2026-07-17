using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementSystem.Models;

public partial class MenuItem
{
    [Key]
    public int ItemId { get; set; }

    [StringLength(100)]
    public string ItemName { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public bool AvailabilityStatus { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("MenuItems")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Item")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
