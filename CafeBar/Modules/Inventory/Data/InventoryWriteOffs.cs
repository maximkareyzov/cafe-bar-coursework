using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeBar.Modules.StaffManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.Inventory.Data;

/// <summary>Ручное списание ингредиента (таблица InventoryWriteOffs).</summary>
public class InventoryWriteOffs
{
    [Key]
    public int WriteOffID { get; set; }

    [ForeignKey(nameof(Employee))]
    public int EmployeeID { get; set; }

    [ForeignKey(nameof(Ingredient))]
    [Column("IngridientID")]
    public int IngredientID { get; set; }

    public decimal Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Employees Employee { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Ingredients Ingredient { get; set; } = null!;
}
