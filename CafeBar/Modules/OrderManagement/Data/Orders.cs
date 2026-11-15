using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeBar.Modules.StaffManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.OrderManagement.Data;

/// <summary>Заказ (таблица Orders).</summary>
public class Orders
{
    [Key]
    public int OrderID { get; set; }

    [ForeignKey(nameof(Employee))]
    public int EmployeeID { get; set; }

    /// <summary>Стол; null, если заказ не привязан к столу (барная стойка, навынос).</summary>
    public string? TableNumber { get; set; }

    public string Status { get; set; } = string.Empty;

    /// <summary>Способ оплаты; null, пока заказ не оплачен.</summary>
    public string? PaymentType { get; set; }

    public decimal DiscountPercent { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>Время оплаты; null, пока заказ не оплачен.</summary>
    public DateTime? PaidAt { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Employees Employee { get; set; } = null!;

    public ICollection<OrderItems> OrderItems { get; } = new List<OrderItems>();
}
