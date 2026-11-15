using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeBar.Modules.MenuManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.OrderManagement.Data;

/// <summary>Модификатор, применённый к позиции заказа (таблица OrderItemModifiers).</summary>
public class OrderItemModifiers
{
    [Key]
    public int OrderItemModifierID { get; set; }

    [ForeignKey(nameof(OrderItem))]
    public int OrderItemID { get; set; }

    [ForeignKey(nameof(Modifier))]
    public int ModifierID { get; set; }

    public decimal PriceExtra { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public OrderItems OrderItem { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Modifiers Modifier { get; set; } = null!;
}
