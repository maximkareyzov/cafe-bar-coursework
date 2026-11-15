using System.ComponentModel.DataAnnotations;

namespace CafeBar.Modules.MenuManagement.Data;

/// <summary>Категория меню (таблица Categories).</summary>
public class Categories
{
    [Key]
    public int CategoryID { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<MenuItems> MenuItems { get; } = new List<MenuItems>();
}
