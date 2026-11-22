using System;
using CafeBar.Data;
using CafeBar.Modules.MenuManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.MenuManagement.Logic;

public class MenuService
{
    public List<Categories> GetCategories()
    {
        using var db = AppDatabase.CreateContext();
        // сортируем в коде, SQLite плохо сортирует кириллицу
        return db.Categories.AsNoTracking().ToList()
            .OrderBy(c => c.Name, StringComparer.CurrentCulture)
            .ToList();
    }

    public List<MenuItems> GetMenuItems()
    {
        using var db = AppDatabase.CreateContext();
        return db.MenuItems.AsNoTracking().Include(m => m.Category).ToList()
            .OrderBy(m => m.Category.Name, StringComparer.CurrentCulture)
            .ThenBy(m => m.Name, StringComparer.CurrentCulture)
            .ToList();
    }

    // id == null - новая категория, иначе меняем существующую
    public bool TrySaveCategory(int? id, string name, out string error)
    {
        name = name.Trim();
        error = string.Empty;
        if (name.Length == 0)
        {
            error = "Укажите название категории.";
            return false;
        }

        using var db = AppDatabase.CreateContext();
        // повторы проверяем в коде, в базе такого ограничения нет
        bool taken = db.Categories.AsNoTracking().ToList()
            .Any(c => c.CategoryID != id && SameName(c.Name, name));
        if (taken)
        {
            error = $"Категория «{name}» уже есть.";
            return false;
        }

        if (id == null)
        {
            db.Categories.Add(new Categories { Name = name });
        }
        else
        {
            var category = db.Categories.SingleOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                error = "Категория не найдена: возможно, её уже удалили.";
                return false;
            }
            category.Name = name;
        }

        db.SaveChanges();
        return true;
    }

    public bool TryDeleteCategory(int id, out string error)
    {
        error = string.Empty;
        using var db = AppDatabase.CreateContext();
        if (db.MenuItems.Any(m => m.CategoryID == id))
        {
            error = "В категории есть позиции меню. Сначала удалите их или перенесите в другую категорию.";
            return false;
        }

        var category = db.Categories.SingleOrDefault(c => c.CategoryID == id);
        if (category == null) return true;

        db.Categories.Remove(category);
        db.SaveChanges();
        return true;
    }

    public bool TrySaveMenuItem(int? id, int categoryId, string name, decimal price, string unit,
        string description, bool isAvailable, out string error)
    {
        name = name.Trim();
        unit = unit.Trim();
        description = description.Trim();
        error = string.Empty;

        if (name.Length == 0) error = "Укажите название позиции.";
        else if (price < 0) error = "Цена не может быть отрицательной.";
        else if (unit.Length == 0) error = "Укажите единицу измерения, например «шт» или «мл».";
        if (error.Length > 0) return false;

        using var db = AppDatabase.CreateContext();
        if (!db.Categories.Any(c => c.CategoryID == categoryId))
        {
            error = "Выбранная категория не найдена.";
            return false;
        }

        // одинаковые названия допустимы только в разных категориях
        bool taken = db.MenuItems.AsNoTracking().ToList()
            .Any(m => m.MenuItemID != id && m.CategoryID == categoryId && SameName(m.Name, name));
        if (taken)
        {
            error = $"Позиция «{name}» в этой категории уже есть.";
            return false;
        }

        if (id == null)
        {
            db.MenuItems.Add(new MenuItems
            {
                CategoryID = categoryId,
                Name = name,
                Price = price,
                Unit = unit,
                Description = description,
                IsAvailable = isAvailable
            });
        }
        else
        {
            var item = db.MenuItems.SingleOrDefault(m => m.MenuItemID == id);
            if (item == null)
            {
                error = "Позиция не найдена: возможно, её уже удалили.";
                return false;
            }
            item.CategoryID = categoryId;
            item.Name = name;
            item.Price = price;
            item.Unit = unit;
            item.Description = description;
            item.IsAvailable = isAvailable;
        }

        db.SaveChanges();
        return true;
    }

    public void SetAvailable(int id, bool isAvailable)
    {
        using var db = AppDatabase.CreateContext();
        var item = db.MenuItems.SingleOrDefault(m => m.MenuItemID == id);
        if (item == null) return;
        item.IsAvailable = isAvailable;
        db.SaveChanges();
    }

    public bool TryDeleteMenuItem(int id, out string error)
    {
        error = string.Empty;
        using var db = AppDatabase.CreateContext();
        var item = db.MenuItems.SingleOrDefault(m => m.MenuItemID == id);
        if (item == null) return true;

        db.MenuItems.Remove(item);
        try
        {
            db.SaveChanges();
        }
        catch (DbUpdateException)
        {
            // позиция уже есть в заказах, внешний ключ не даёт удалить
            error = "Позицию нельзя удалить: она есть в заказах. Сделайте её недоступной.";
            return false;
        }

        return true;
    }

    private static bool SameName(string first, string second)
    {
        return string.Equals(first, second, StringComparison.CurrentCultureIgnoreCase);
    }
}
