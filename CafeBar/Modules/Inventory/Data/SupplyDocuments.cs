using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeBar.Modules.StaffManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.Inventory.Data;

/// <summary>Документ поступления товара от поставщика (таблица SupplyDocuments).</summary>
public class SupplyDocuments
{
    [Key]
    public int SupplyDocumentID { get; set; }

    [ForeignKey(nameof(Supplier))]
    public int SupplierID { get; set; }

    [ForeignKey(nameof(Employee))]
    public int EmployeeID { get; set; }

    public DateTime SupplyDate { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Suppliers Supplier { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Employees Employee { get; set; } = null!;

    public ICollection<SupplyItems> SupplyItems { get; } = new List<SupplyItems>();
}
