using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeBar.Modules.StaffManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.Administration.Data;

/// <summary>Запись журнала действий пользователей (таблица AuditLogs).</summary>
public class AuditLogs
{
    [Key]
    public int LogID { get; set; }

    [ForeignKey(nameof(Employee))]
    public int EmployeeID { get; set; }

    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Employees Employee { get; set; } = null!;
}
