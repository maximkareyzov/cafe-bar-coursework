using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.Inventory.Data;

/// <summary>Позиция документа поступления (таблица SupplyItems).</summary>
public class SupplyItems
{
    /// <summary>Строковый ключ, как на диаграммах (varchar). Значение — новый GUID.</summary>
    [Key]
    public string SupplyItemID { get; set; } = Guid.NewGuid().ToString();

    [ForeignKey(nameof(SupplyDocument))]
    public int SupplyDocumentID { get; set; }

    [ForeignKey(nameof(Ingredient))]
    [Column("IngridientID")]
    public int IngredientID { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime ExpirationDate { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public SupplyDocuments SupplyDocument { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Ingredients Ingredient { get; set; } = null!;
}
