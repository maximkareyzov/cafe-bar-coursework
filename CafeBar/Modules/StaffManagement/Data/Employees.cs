using System.ComponentModel.DataAnnotations;

namespace CafeBar.Modules.StaffManagement.Data;

/// <summary>Сотрудник (таблица Employees).</summary>
public class Employees
{
    [Key]
    public int EmployeeID { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public decimal SalesPercent { get; set; }
}
