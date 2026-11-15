using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeBar.Modules.Inventory.Data;

/// <summary>Ингредиент на складе. На ER-диаграмме таблица называется Ingridients.</summary>
[Table("Ingridients")]
public class Ingredients
{
    [Key]
    [Column("IngridientID")]
    public int IngredientID { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;

    /// <summary>Текущий остаток. На диаграммах тип varchar, поэтому число хранится строкой.</summary>
    public string CurrentStock { get; set; } = "0";

    [Column("MinStockThresold")]
    public decimal MinStockThreshold { get; set; }

    public decimal AvgPurchasePrice { get; set; }
}
