using System.ComponentModel.DataAnnotations;

namespace CafeBar.Modules.Inventory.Data;

/// <summary>Поставщик (таблица Suppliers).</summary>
public class Suppliers
{
    [Key]
    public int SupplierID { get; set; }

    public string Name { get; set; } = string.Empty;
    public string INN { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
}
