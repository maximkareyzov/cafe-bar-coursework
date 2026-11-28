using CafeBar.Data;
using CafeBar.Modules.Inventory.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.Inventory.Logic;

public class SupplierService
{
    public List<Suppliers> GetAll()
    {
        using var db = AppDatabase.CreateContext();
        return db.Suppliers.AsNoTracking().ToList()
            .OrderBy(s => s.Name, StringComparer.CurrentCulture)
            .ToList();
    }

    // id == null - новый поставщик
    public bool TrySave(int? id, string name, string inn, string contactInfo, out string error)
    {
        name = name.Trim();
        inn = inn.Trim();
        contactInfo = contactInfo.Trim();
        error = string.Empty;

        if (name.Length == 0) error = "Укажите название поставщика.";
        else if (!IsInn(inn)) error = "ИНН должен состоять из 10 или 12 цифр.";
        if (error.Length > 0) return false;

        using var db = AppDatabase.CreateContext();
        // повторы проверяем в коде, в базе такого ограничения нет
        bool taken = db.Suppliers.AsNoTracking().ToList()
            .Any(s => s.SupplierID != id && s.INN == inn);
        if (taken)
        {
            error = "Поставщик с таким ИНН уже есть.";
            return false;
        }

        if (id == null)
        {
            db.Suppliers.Add(new Suppliers { Name = name, INN = inn, ContactInfo = contactInfo });
        }
        else
        {
            var supplier = db.Suppliers.SingleOrDefault(s => s.SupplierID == id);
            if (supplier == null)
            {
                error = "Поставщик не найден: возможно, его уже удалили.";
                return false;
            }
            supplier.Name = name;
            supplier.INN = inn;
            supplier.ContactInfo = contactInfo;
        }

        db.SaveChanges();
        return true;
    }

    public bool TryDelete(int id, out string error)
    {
        error = string.Empty;
        using var db = AppDatabase.CreateContext();
        var supplier = db.Suppliers.SingleOrDefault(s => s.SupplierID == id);
        if (supplier == null) return true;

        db.Suppliers.Remove(supplier);
        try
        {
            db.SaveChanges();
        }
        catch (DbUpdateException)
        {
            // по поставщику уже есть поступления
            error = "Поставщика нельзя удалить: по нему есть поступления.";
            return false;
        }

        return true;
    }

    private static bool IsInn(string inn)
    {
        return (inn.Length == 10 || inn.Length == 12) && inn.All(char.IsDigit);
    }
}