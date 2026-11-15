using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.MenuManagement.Data;

/// <summary>Позиция меню (таблица MenuItems).</summary>
public class MenuItems
{
    [Key]
    public int MenuItemID { get; set; }

    [ForeignKey(nameof(Category))]
    public int CategoryID { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Categories Category { get; set; } = null!;

    public ICollection<Modifiers> Modifiers { get; } = new List<Modifiers>();
}
