using CafeBar.Modules.Administration.Data;
using CafeBar.Modules.Inventory.Data;
using CafeBar.Modules.MenuManagement.Data;
using CafeBar.Modules.OrderManagement.Data;
using CafeBar.Modules.StaffManagement.Data;
using CafeBar.Modules.TechCards.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Data;

/// <summary>
/// Общий контекст базы данных. Классы названы как на диаграмме классов и лежат в папках Data
/// своих модулей. Ключи, связи, правила удаления и имена таблиц заданы там же атрибутами.
/// </summary>
public class CafeBarDbContext : DbContext
{
    public CafeBarDbContext(DbContextOptions<CafeBarDbContext> options)
        : base(options)
    {
    }

    // Управление персоналом
    public DbSet<Employees> Employees => Set<Employees>();
    public DbSet<Shifts> Shifts => Set<Shifts>();

    // Администрирование
    public DbSet<AuditLogs> AuditLogs => Set<AuditLogs>();

    // Управление заказами
    public DbSet<Orders> Orders => Set<Orders>();
    public DbSet<OrderItems> OrderItems => Set<OrderItems>();
    public DbSet<OrderItemModifiers> OrderItemModifiers => Set<OrderItemModifiers>();

    // Управление меню
    public DbSet<Categories> Categories => Set<Categories>();
    public DbSet<MenuItems> MenuItems => Set<MenuItems>();
    public DbSet<Modifiers> Modifiers => Set<Modifiers>();

    // Технологические карты
    public DbSet<RecipItems> RecipItems => Set<RecipItems>();

    // Складской учёт
    public DbSet<Ingredients> Ingredients => Set<Ingredients>();
    public DbSet<InventoryWriteOffs> InventoryWriteOffs => Set<InventoryWriteOffs>();
    public DbSet<Suppliers> Suppliers => Set<Suppliers>();
    public DbSet<SupplyDocuments> SupplyDocuments => Set<SupplyDocuments>();
    public DbSet<SupplyItems> SupplyItems => Set<SupplyItems>();

    // Типы столбцов названы как на ER-диаграмме. SQLite принимает такие имена типов.
    // Целые числа остаются INTEGER: только с таким типом работает автоинкремент первичного ключа.
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveColumnType("varchar");
        configurationBuilder.Properties<decimal>().HaveColumnType("decimal");
        configurationBuilder.Properties<DateTime>().HaveColumnType("datetime");
        configurationBuilder.Properties<bool>().HaveColumnType("boolean");
    }
}
