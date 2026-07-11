using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Restaurant_System.Models;

[PrimaryKey("OrderId", "ItemId")]
public partial class OrderDetail
{
    [Key]
    public int OrderId { get; set; }

    [Key]
    public int ItemId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal UnitPrice { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("OrderDetails")]
    public virtual MenuItem Item { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;


    public string OrderDisplay()
    {
        return $"Order #{OrderId}";
    }
}
