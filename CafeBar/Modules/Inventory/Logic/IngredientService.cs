using System.Globalization;
using CafeBar.Data;
using CafeBar.Modules.Inventory.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.Inventory.Logic;

// строка списка склада: ингредиент с уже разобранным остатком
public class IngredientRow
{
    public int IngredientID { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Unit { get; init; } = string.Empty;
    public decimal Stock { get; init; }
    public decimal MinThreshold { get; init; }
    public decimal AvgPrice { get; init; }

    public bool IsLow => MinThreshold > 0 && Stock < MinThreshold;

    public string Status => IsLow ? "Ниже порога" : string.Empty;
}

public class IngredientService
{
    public static IReadOnlyList<string> Units { get; } = new[] { "г", "кг", "мл", "л", "шт" };

    // остаток в базе лежит строкой, поэтому разбираем её в число
    public static decimal ParseStock(string text)
    {
        bool ok = decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value);
        return ok ? value : 0;
    }

    public List<IngredientRow> GetRows(string search, bool onlyLow)
    {
        search = search.Trim();
        using var db = AppDatabase.CreateContext();
        // поиск и сортировку делаем в коде, SQLite плохо работает с кириллицей
        return db.Ingredients.AsNoTracking().ToList()
            .Select(i => new IngredientRow
            {
                IngredientID = i.IngredientID,
                Name = i.Name,
                Unit = i.Unit,
                Stock = ParseStock(i.CurrentStock),
                MinThreshold = i.MinStockThreshold,
                AvgPrice = i.AvgPurchasePrice
            })
            .Where(r => search.Length == 0 || r.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase))
            .Where(r => !onlyLow || r.IsLow)
            .OrderBy(r => r.Name, StringComparer.CurrentCulture)
            .ToList();
    }

    public Ingredients? Get(int id)
    {
        using var db = AppDatabase.CreateContext();
        return db.Ingredients.AsNoTracking().SingleOrDefault(i => i.IngredientID == id);
    }

    // id == null - новый ингредиент, иначе меняем существующий
    public bool TrySave(int? id, string name, string unit, decimal stock, decimal minThreshold, out string error)
    {
        name = name.Trim();
        error = string.Empty;

        if (name.Length == 0) error = "Укажите название ингредиента.";
        else if (!Units.Contains(unit)) error = "Выберите единицу измерения.";
        else if (stock < 0) error = "Остаток не может быть отрицательным.";
        else if (minThreshold < 0) error = "Минимальный порог не может быть отрицательным.";
        if (error.Length > 0) return false;

        using var db = AppDatabase.CreateContext();
        bool taken = db.Ingredients.AsNoTracking().ToList()
            .Any(i => i.IngredientID != id && string.Equals(i.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (taken)
        {
            error = $"Ингредиент «{name}» уже есть.";
            return false;
        }

        string stockText = stock.ToString(CultureInfo.InvariantCulture);
        if (id == null)
        {
            db.Ingredients.Add(new Ingredients
            {
                Name = name,
                Unit = unit,
                CurrentStock = stockText,
                MinStockThreshold = minThreshold
            });
        }
        else
        {
            var ingredient = db.Ingredients.SingleOrDefault(i => i.IngredientID == id);
            if (ingredient == null)
            {
                error = "Ингредиент не найден: возможно, его уже удалили.";
                return false;
            }
            ingredient.Name = name;
            ingredient.Unit = unit;
            ingredient.CurrentStock = stockText;
            ingredient.MinStockThreshold = minThreshold;
        }

        db.SaveChanges();
        return true;
    }

    public bool TryDelete(int id, out string error)
    {
        error = string.Empty;
        using var db = AppDatabase.CreateContext();
        var ingredient = db.Ingredients.SingleOrDefault(i => i.IngredientID == id);
        if (ingredient == null) return true;

        db.Ingredients.Remove(ingredient);
        try
        {
            db.SaveChanges();
        }
        catch (DbUpdateException)
        {
            // ингредиент уже есть в техкарте, поступлении или списании
            error = "Ингредиент нельзя удалить: он используется в техкартах, поступлениях или списаниях.";
            return false;
        }

        return true;
    }
}