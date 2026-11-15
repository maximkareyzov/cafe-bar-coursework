using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeBar.Modules.Inventory.Data;
using CafeBar.Modules.MenuManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.TechCards.Data;

/// <summary>
/// Строка техкарты: ингредиент и его количество в позиции меню.
/// На ER-диаграмме таблица называется RecipeItems.
/// </summary>
[Table("RecipeItems")]
public class RecipItems
{
    [Key]
    [Column("RecipeItemID")]
    public int RecipItemID { get; set; }

    [ForeignKey(nameof(MenuItem))]
    public int MenuItemID { get; set; }

    [ForeignKey(nameof(Ingredient))]
    [Column("IngridientID")]
    public int IngredientID { get; set; }

    public decimal Amount { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public MenuItems MenuItem { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Ingredients Ingredient { get; set; } = null!;
}
