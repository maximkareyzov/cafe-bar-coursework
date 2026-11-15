using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.MenuManagement.Data;

/// <summary>Модификатор позиции меню (таблица Modifiers).</summary>
public class Modifiers
{
    [Key]
    public int ModifierID { get; set; }

    [ForeignKey(nameof(MenuItem))]
    public int MenuItemID { get; set; }

    public string GroupName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal PriceExtra { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public MenuItems MenuItem { get; set; } = null!;
}
