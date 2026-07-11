using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Restaurant_System.Models;

public partial class RestaurantTable
{
    [Key]
    public int TableId { get; set; }

    public int Capacity { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [InverseProperty("Table")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [NotMapped]
    public string TableDisplay
    {
        get
        {
            return $"Table {TableId} ({Status})";
        }
    }
}
