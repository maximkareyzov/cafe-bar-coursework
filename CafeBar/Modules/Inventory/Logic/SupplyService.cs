using System.Globalization;
using CafeBar.Data;
using CafeBar.Modules.Inventory.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.Inventory.Logic;

// строка списка документов
public class SupplyRow
{
    public int SupplyDocumentID { get; init; }
    public DateTime SupplyDate { get; init; }
    public string SupplierName { get; init; } = string.Empty;
    public string EmployeeName { get; init; } = string.Empty;
    public int ItemCount { get; init; }
    public decimal Total { get; init; }
}

// позиция уже проведённого документа
public class SupplyLineRow
{
    public string IngredientName { get; init; } = string.Empty;
    public string Unit { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public DateTime ExpirationDate { get; init; }
    public decimal Sum => Quantity * UnitPrice;
}

// позиция нового документа, пока он не проведён
public class SupplyLine
{
    public int IngredientID { get; init; }
    public string IngredientName { get; init; } = string.Empty;
    public string Unit { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public DateTime ExpirationDate { get; init; }
    public decimal Sum => Quantity * UnitPrice;
}

public class SupplyService
{
    public List<SupplyRow> GetDocuments()
    {
        using var db = AppDatabase.CreateContext();
        return db.SupplyDocuments.AsNoTracking()
            .Include(d => d.Supplier)
            .Include(d => d.Employee)
            .Include(d => d.SupplyItems)
            .ToList()
            .Select(d => new SupplyRow
            {
                SupplyDocumentID = d.SupplyDocumentID,
                SupplyDate = d.SupplyDate,
                SupplierName = d.Supplier.Name,
                EmployeeName = d.Employee.FullName,
                ItemCount = d.SupplyItems.Count,
                Total = d.SupplyItems.Sum(i => i.Quantity * i.UnitPrice)
            })
            .OrderByDescending(r => r.SupplyDate)
            .ThenByDescending(r => r.SupplyDocumentID)
            .ToList();
    }

    public List<SupplyLineRow> GetLines(int documentId)
    {
        using var db = AppDatabase.CreateContext();
        return db.SupplyItems.AsNoTracking()
            .Include(i => i.Ingredient)
            .Where(i => i.SupplyDocumentID == documentId)
            .ToList()
            .Select(i => new SupplyLineRow
            {
                IngredientName = i.Ingredient.Name,
                Unit = i.Ingredient.Unit,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                ExpirationDate = i.ExpirationDate
            })
            .OrderBy(r => r.IngredientName, StringComparer.CurrentCulture)
            .ToList();
    }

    // проводим документ: остатки растут, средняя цена пересчитывается
    public bool TryPost(int supplierId, DateTime date, IReadOnlyList<SupplyLine> lines, int employeeId, out string error)
    {
        error = string.Empty;
        if (lines.Count == 0)
        {
            error = "Добавьте хотя бы одну позицию.";
            return false;
        }

        using var db = AppDatabase.CreateContext();
        if (!db.Suppliers.Any(s => s.SupplierID == supplierId))
        {
            error = "Поставщик не найден.";
            return false;
        }

        var document = new SupplyDocuments
        {
            SupplierID = supplierId,
            EmployeeID = employeeId,
            SupplyDate = date.Date
        };

        foreach (var line in lines)
        {
            if (line.Quantity <= 0 || line.UnitPrice < 0 || line.ExpirationDate.Date < date.Date)
            {
                error = $"Позиция «{line.IngredientName}» заполнена неверно.";
                return false;
            }

            var ingredient = db.Ingredients.SingleOrDefault(i => i.IngredientID == line.IngredientID);
            if (ingredient == null)
            {
                error = $"Ингредиент «{line.IngredientName}» не найден.";
                return false;
            }

            decimal oldStock = IngredientService.ParseStock(ingredient.CurrentStock);
            decimal newStock = oldStock + line.Quantity;
            // средневзвешенная закупочная цена
            decimal oldCost = oldStock * ingredient.AvgPurchasePrice;
            ingredient.AvgPurchasePrice = Math.Round((oldCost + line.Quantity * line.UnitPrice) / newStock, 2);
            ingredient.CurrentStock = newStock.ToString(CultureInfo.InvariantCulture);

            document.SupplyItems.Add(new SupplyItems
            {
                IngredientID = line.IngredientID,
                Quantity = line.Quantity,
                UnitPrice = line.UnitPrice,
                ExpirationDate = line.ExpirationDate.Date
            });
        }

        db.SupplyDocuments.Add(document);
        // одно сохранение - одна транзакция, документ и остатки меняются вместе
        db.SaveChanges();
        return true;
    }
}