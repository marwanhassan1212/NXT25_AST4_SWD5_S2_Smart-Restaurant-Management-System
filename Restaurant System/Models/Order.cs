using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Restaurant_System.Models;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [StringLength(30)]
    public string OrderStatus { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalAmount { get; set; }

    public int EmployeeId { get; set; }

    public int CustomerId { get; set; }

    public int TableId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("EmployeeId")]
    [InverseProperty("Orders")]
    public virtual Employee Employee { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("TableId")]
    [InverseProperty("Orders")]
    public virtual RestaurantTable Table { get; set; } = null!;

    

[NotMapped]
public string OrderDisplay
{
    get
    {
        return $"Order #{OrderId}";
    }
}
}
