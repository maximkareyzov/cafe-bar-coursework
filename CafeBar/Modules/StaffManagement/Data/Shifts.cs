using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.StaffManagement.Data;

/// <summary>Время прихода и ухода сотрудника (таблица Shifts).</summary>
public class Shifts
{
    [Key]
    public int ShiftID { get; set; }

    [ForeignKey(nameof(Employee))]
    public int EmployeeID { get; set; }

    public DateTime ClockIn { get; set; }

    /// <summary>Время ухода; null, пока уход не зарегистрирован.</summary>
    public DateTime? ClockOut { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Employees Employee { get; set; } = null!;
}
