using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeBar.Modules.MenuManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.OrderManagement.Data;

/// <summary>Позиция заказа (таблица OrderItems).</summary>
public class OrderItems
{
    [Key]
    public int OrderItemID { get; set; }

    [ForeignKey(nameof(Order))]
    public int OrderID { get; set; }

    [ForeignKey(nameof(MenuItem))]
    public int MenuItemID { get; set; }

    public int Quantity { get; set; }

    /// <summary>Цена, зафиксированная в момент добавления позиции в заказ.</summary>
    public decimal UnitPrice { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Orders Order { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public MenuItems MenuItem { get; set; } = null!;

    public ICollection<OrderItemModifiers> OrderItemModifiers { get; } = new List<OrderItemModifiers>();
}
